using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Helper_Detmach
{
    public class SitemapBuilder
    {
        

        private List<SitemapUrl> _urls;
        private bool image = false;
        private bool sitemapindex = false;
        public SitemapBuilder(bool image = false, bool sitemapindex = false)
        {
            _urls = new List<SitemapUrl>();
            this.image = image;
            this.sitemapindex = sitemapindex;
        }
        public void AddUrl(string url, DateTime? modified = null, ChangeFrequency? changeFrequency = null, double? priority = null, List<Imagemap> image = default)
        {
            _urls.Add(new SitemapUrl()
            {
                Url = url,
                Modified = modified,
                ChangeFrequency = changeFrequency,
                Priority = priority,
                Images = image,
            });
        }
        public override string ToString()
        {
            CultureInfo culture = new CultureInfo("en-US");
            StringBuilder strBuilder = new StringBuilder();
            if (sitemapindex)
            {
                strBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                strBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"/sitemap.xsl\"?>");
                strBuilder.Append("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">");
                _urls.ForEach(p =>
                {
                    strBuilder.Append("<sitemap>");
                    strBuilder.Append("<loc>");
                    strBuilder.Append(p.Url);
                    strBuilder.Append("</loc>");
                    strBuilder.Append("<lastmod>");
                    strBuilder.Append($"{p.Modified.Value.ToString("yyyy-MM-dd")}");
                    strBuilder.Append("</lastmod>");
                    strBuilder.Append("</sitemap>");
                });
                strBuilder.Append("</sitemapindex>");
                return strBuilder.ToString();
            }
            if (image)
            {
                strBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                strBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"/sitemap.xsl\"?>");
                strBuilder.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\" >");
                _urls.ForEach(p =>
                {
                    strBuilder.Append("<url>");
                    strBuilder.Append("<loc>");
                    strBuilder.Append($"{p.Url}");
                    strBuilder.Append("</loc>");
                    strBuilder.Append("<changefreq>");
                    strBuilder.Append(p.ChangeFrequency.ToString());
                    strBuilder.Append("</changefreq>");
                    strBuilder.Append("<priority>");
                    strBuilder.Append(p.Priority);
                    strBuilder.Append("</priority>");
                    strBuilder.Append("<lastmod>");
                    strBuilder.Append($"{p.Modified.Value.ToString("yyyy-MM-dd")}");
                    strBuilder.Append("</lastmod>");
                    if(p.Images != null)
                    foreach (var im in p.Images)
                    {
                        strBuilder.Append("<image:image>");
                        strBuilder.Append("<image:loc>");
                        strBuilder.Append($"{im.Url}");
                        strBuilder.Append("</image:loc>");
                        strBuilder.Append("<image:caption>");
                        strBuilder.Append($"<![CDATA[{im.Caption}]]>");
                        strBuilder.Append("</image:caption>");
                        strBuilder.Append("<image:title>");
                        strBuilder.Append($"<![CDATA[{im.Title}]]>");
                        strBuilder.Append("</image:title>");
                        strBuilder.Append("</image:image>");
                    }                    
                    strBuilder.Append("</url>");
                });
            }
            else
            {
                strBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                strBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"/sitemap.xsl\"?>");
                strBuilder.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">");
                _urls.ForEach(p =>
                {
                    strBuilder.Append("<url>");
                    strBuilder.Append("<loc>");
                    strBuilder.Append($"{p.Url}");
                    strBuilder.Append("</loc>");
                    strBuilder.Append("<changefreq>");
                    strBuilder.Append(p.ChangeFrequency.ToString());
                    strBuilder.Append("</changefreq>");
                    strBuilder.Append("<priority>");
                    strBuilder.Append(p.Priority);
                    strBuilder.Append("</priority>");
                    strBuilder.Append("<lastmod>");
                    strBuilder.Append($"{p.Modified.Value.ToString("yyyy-MM-dd")}");
                    strBuilder.Append("</lastmod>");
                    strBuilder.Append("</url>");
                });
            }
            strBuilder.Append("</urlset>");
            return strBuilder.ToString();
        }
    }

    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
    public class SitemapUrl
    {
        public string Url { get; set; }
        public DateTime? Modified { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public double? Priority { get; set; }
        public List<Imagemap> Images { get; set; }
    }
    public class Imagemap
    {
        public string Url { get; set; }
        public string Caption { get; set; }
        public string Title { get; set; }
    }
}


