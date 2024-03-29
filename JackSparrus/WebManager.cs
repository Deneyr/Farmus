﻿using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackSparrus
{
    public class WebManager
    {
        IWebDriver driver;

        public WebManager()
        {
            FirefoxOptions option = new FirefoxOptions();
            option.AddArgument("--headless");
            this.driver = new FirefoxDriver(option);

            driver.Navigate().GoToUrl("https://dofus-map.com/fr/hunt");
        }

        public int GetHintDistance(Point startPoint, Direction direction, string hint, out string mostAccurateHint)
        {           
            Console.WriteLine(String.Join("\t", new string[] { startPoint.X.ToString(), startPoint.Y.ToString(), TreasureHub.GetDirectionString(direction) }));
            /*IWebElement queryX = driver.FindElement(By.Id("x"));
            queryX.SendKeys(startPoint.X.ToString());

            IWebElement queryY = driver.FindElement(By.Id("y"));
            queryY.SendKeys(startPoint.Y.ToString());*/
            /*
            IWebElement queryDirection = driver.FindElement(By.Id(TreasureHub.GetDirectionString(direction)));
            Console.WriteLine(queryDirection.Enabled);
            queryDirection.Click();*/

            IJavaScriptExecutor jsExecuter = (IJavaScriptExecutor)driver;

            // Prevent world prompt
            jsExecuter.ExecuteScript("function coordonnatesAreConflictual(x, y, node) { world = 0; return false; }");
            
            // Set pos
            jsExecuter.ExecuteScript(String.Format("document.querySelector('#x').value = {0};", startPoint.X.ToString()));
            jsExecuter.ExecuteScript(String.Format("document.querySelector('#y').value = {0};", startPoint.Y.ToString()));

            // Set direction
            jsExecuter.ExecuteScript(String.Format("setDirection(document.querySelector('#{0}'));", TreasureHub.GetDirectionString(direction)));

            // Wait for hints loading
            while ((long) jsExecuter.ExecuteScript("return document.querySelector(\"#hintName\").length") == 1);

            IWebElement hintsElement = driver.FindElement(By.Id("hintName"));
            IEnumerable<IWebElement> hints = hintsElement.FindElements(By.TagName("option"));

            List<string> hintTexts = hints.Where(pElem => pElem.GetAttribute("value") != "null").Select(pElem => pElem.Text).ToList();

            if (hintTexts.Count == 0)
                throw new Exception("Erreur dans la récupération de l'indice en ligne");

            mostAccurateHint = ReturnMostAccurateString(hint, hintTexts);

            if(hintTexts.Contains(hint) && mostAccurateHint != hint)
            {
                Console.WriteLine();
            }

            new SelectElement(hintsElement).SelectByText(mostAccurateHint);

            IWebElement resultElement = driver.FindElement(By.Id("resultDistance"));
            string text = resultElement.Text;

            return int.Parse(text);
        }

        //private List<string> ParseHtml(string html)
        //{
        //    HtmlDocument htmlDoc = new HtmlDocument();
        //    htmlDoc.LoadHtml(html);
        //    IEnumerable<HtmlNode> optionNodes = htmlDoc.GetElementbyId("hintName").Elements("option");

        //    foreach(HtmlNode hintNode in optionNodes)
        //    {

        //    }
        //}

        public static string ReturnMostAccurateString(string hint, List<string> strings)
        {
            List<int> matchList = new List<int>();
            foreach (string Track in strings)
            {
                matchList.Add(LevenshteinDistance(Track, hint));
            }
            return strings.ElementAt(matchList.IndexOf(matchList.Min()));
        }

        public static int LevenshteinDistance(string source, string target)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target)) return 0;
                return target.Length;
            }
            if (String.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++) distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                    distance[currentRow, j] = Math.Min(Math.Min(
                        distance[previousRow, j] + 1,
                        distance[currentRow, j - 1] + 1),
                        distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }
    }

}
