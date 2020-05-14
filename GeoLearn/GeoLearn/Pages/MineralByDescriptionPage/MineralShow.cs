using GeoLearn.Database;
using GeoLearn.Extensions;
using GeoLearn.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace GeoLearn.Pages
{
    class MineralShow
    {
        private Dictionary<string, Label> Labels = new Dictionary<string, Label>();
        public Mineral currectMineral { get; private set; }
        public Frame frame { get; private set; }
        public MineralShow(Mineral mineral, ref StackLayout layout, string padd, int extraPadding = 10,
            string[] showParametrs = null, string[] dontShowParametrs = null, string lenguage = "ru")
        {
            List<string> showList = new List<string>(),
                showListTranslated = showList;

            if (showParametrs == null)
                showList = new List<string>(Mineral.parametrsList);
            else
                showList = new List<string>(showParametrs);

            if (lenguage == "ru")
                showListTranslated = new List<string>(Mineral.parametrsListRU);

            if (dontShowParametrs != null)
                foreach (var parametr in dontShowParametrs)
                {
                    int pos = showList.IndexOf(parametr);
                    showList.RemoveAt(pos);
                    showListTranslated.RemoveAt(pos);
                }

            currectMineral = mineral;

            //try
            //{
            double[] padding = Array.ConvertAll(padd.Split(','), (string x) => double.Parse(x.Trim()));
            Thickness thickness = new Thickness(padding[0] + extraPadding, 0, padding[2], 0);

            // Создаем новый StackLayout для Frame
            StackLayout labelsContent = new StackLayout();
            //ScrollView scrollView = new ScrollView()
            //{
            //    Content = labelsContent
            //};

            for (int parametrNum = 0; parametrNum < showList.Count; parametrNum++)
            {
                Labels[showList[parametrNum]] = new Label()
                {
                    Text = $"{showListTranslated[parametrNum]}: " + mineral[showList[parametrNum]],
                    Padding = thickness,
                    BackgroundColor = Colorset.BackgroundColor,
                    TextColor = Colorset.Text
                };

                labelsContent.Children.Add(Labels[showList[parametrNum]]);
            }
            // Добавялем отступ к первому элементу
            Labels[showList[0]].Padding = new Thickness(padding[0] + extraPadding, padding[1], padding[2], 0);
            if (showList[0] == "name")
            {
                Labels[showList[0]].FontAttributes = FontAttributes.Bold;
                Labels[showList[0]].FontSize += 5;
                Labels[showList[0]].Text = mineral[showList[0]].UpperFirstLetter();
            }
            // Добавялем отступ к последнему элементу
            Labels[showList[showList.Count - 1]].Padding = new Thickness(padding[0] + extraPadding, 0, padding[2], padding[3]);


            frame = new Frame()
            {
                Content = labelsContent,
                BackgroundColor = Colorset.BackgroundColor,
                BorderColor = Colorset.NonImportant,
                HasShadow = true
            };

            layout.Children.Add(frame);
            //}
            //catch (Exception e)
            //{ System.Diagnostics.Debug.WriteLine("Show error: " + e.Message); }
        }

        public void ChangeMineral(Mineral mineral)
        {
            for (int parametrNum = 0; parametrNum < Mineral.parametrsList.Length; parametrNum++)
            {
                if (Labels.ContainsKey(Mineral.parametrsList[parametrNum]))
                    Labels[Mineral.parametrsList[parametrNum]].Text =
                        $"{Mineral.parametrsListRU[parametrNum]}: " +
                        mineral[Mineral.parametrsList[parametrNum]];
            }
        }
    }
}