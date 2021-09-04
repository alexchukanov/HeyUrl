using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeyUrl.Data;
using HeyUrl.Models;
using Shyjus.BrowserDetection;
using HeyUrl.ViewModels;
using HeyUrl.Utils;

namespace HeyUrl.Controllers
{
    public class ClicksController : Controller
    {
        private readonly HeyUrlContext _context;
        private readonly IBrowserDetector _browserDetector;

        public ClicksController(HeyUrlContext context, IBrowserDetector browserDetector)
        {
            _context = context;
            _browserDetector = browserDetector;
        }

        
       [Route("/Clicks/{shortUrl}")]
        public async Task<IActionResult> Visit(string shortUrl)
        {
            if(shortUrl == "Visit" && TempData["ShortUrl"] != null)
			{
                shortUrl = TempData["ShortUrl"].ToString();
            }

            bool isValid = HelperUtil.IsValidShortUrl(shortUrl);

            if (isValid)
            {
                var url = _context.Url.FirstOrDefault(u => u.ShortUrl == shortUrl);

                if (url != null)
                {
                    Click click = new Click
                    {
                        Id = Guid.NewGuid(),
                        ShortUrl = shortUrl,
                        Browser = this._browserDetector.Browser.Name,
                        Platform = this._browserDetector.Browser.OS,
                        Clicked = DateTime.Today
                    };

                    _context.Add(click);
                    await _context.SaveChangesAsync();

                    UriBuilder uriBuilder = new UriBuilder(url.OriginalUrl);
                    return Redirect(uriBuilder.Uri.ToString());
                }
                else
				{
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [Route("Clicks/Show/{url}/{count}")]
        public IActionResult Show(string url, int count)
        {
            if (string.IsNullOrWhiteSpace(url) || url.Length != 5)
            {
                TempData["Notice"] = $"Wrong Url: {url}";
            }
            else if (count == 0)
            {
                TempData["Notice"] = $"Url's {url} clicks count = 0";
            }
            else
            {
                //Url
                int totalCount = _context.Click.Count(c => c.ShortUrl == url);

                var urlStatus = _context.Url.FirstOrDefault(u => u.ShortUrl == url);

                string originalUrl = _context.Url.FirstOrDefault(u => u.ShortUrl == url).OriginalUrl;

                Url viewUrl = new Url { ShortUrl = url, OriginalUrl = urlStatus.OriginalUrl, Created = urlStatus.Created, Count = totalCount };

                int year = DateTime.Today.Year;
                int month = DateTime.Today.Month;
                DateTime firstMonthDay = new DateTime(year, month, 1);

                var clickList = _context.Click.Where(c => c.ShortUrl == url && c.Clicked >= firstMonthDay);


                //DailyClicks
                var dailyClicks = new Dictionary<string, int>();

                var clickDayList = from d in clickList
                                   group d by d.Clicked into dateGroup
                                   select new
                                   {
                                       Day = dateGroup.Key,
                                       Count = dateGroup.Count()
                                   };

                foreach (var clickDay in clickDayList)
                {
                    dailyClicks.Add(clickDay.Day.Day.ToString(), clickDay.Count);
                }


                //BrowserClicks
                var browserClicks = new Dictionary<string, int>();

                var browserList = from b in clickList
                                  group b by b.Browser into bGroup
                                  select new
                                  {
                                      Browser = bGroup.Key,
                                      Count = bGroup.Count()
                                  };

                foreach (var br in browserList)
                {
                    browserClicks.Add(br.Browser, br.Count);
                }


                //PlatformClicks
                var platformClicks = new Dictionary<string, int>();

                var platformList = from p in clickList
                                   group p by p.Platform into pGroup
                                   select new
                                   {
                                       OS = pGroup.Key,
                                       Count = pGroup.Count()
                                   };

                foreach (var pl in platformList)
                {
                    platformClicks.Add(pl.OS, pl.Count);
                }

                var svm = new ShowViewModel();

                svm.Url = viewUrl;
                svm.DailyClicks = dailyClicks;
                svm.BrowserClicks = browserClicks;
                svm.PlatformClicks = platformClicks;

                return View(svm);
            }

            return RedirectToAction("Index", "Urls");
        }

    }
}
