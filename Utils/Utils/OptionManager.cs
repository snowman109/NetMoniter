using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class OptionManager
    {
        public static Option auto = new Option { Name = "自动识别", Value = "auto" };
        public static Option zh = new Option { Name = "简体中文", Value = "zh" };
        public static Option zhTW = new Option { Name = "繁体中文", Value = "zh-TW" };
        public static Option en = new Option { Name = "英语", Value = "en" };
        public static Option ja = new Option { Name = "日语", Value = "ja" };
        public static Option ko = new Option { Name = "韩语", Value = "ko" };
        public static Option fr = new Option { Name = "法语", Value = "fr" };
        public static Option es = new Option { Name = "西班牙语", Value = "es" };
        public static Option it = new Option { Name = "意大利语", Value = "it" };
        public static Option de = new Option { Name = "德语", Value = "de" };
        public static Option tr = new Option { Name = "土耳其语", Value = "tr" };
        public static Option ru = new Option { Name = "俄语", Value = "ru" };
        public static Option pt = new Option { Name = "葡萄牙语", Value = "pt" };
        public static Option vi = new Option { Name = "越南语", Value = "vi" };
        public static Option id = new Option { Name = "印尼语", Value = "id" };
        public static Option th = new Option { Name = "泰语", Value = "th" };
        public static Option ms = new Option { Name = "马来西亚语", Value = "ms" };
        public static Option ar = new Option { Name = "阿拉伯语", Value = "ar" };
        public static Option hi = new Option { Name = "印地语", Value = "hi" };
        public static ObservableCollection<Option> LoadDefaultSource()
        {
            ObservableCollection<Option> source = new ObservableCollection<Option>() {auto, zh, zhTW, en, ja, ko, fr, es, it, de, tr, ru, pt, vi, id, th, ms, ar, hi };
            return source;
        }
        public static Dictionary<string, ObservableCollection<Option>> LoadDefaultTarget()
        {
            Dictionary<string, ObservableCollection<Option>> target = new Dictionary<string, ObservableCollection<Option>>();
            ObservableCollection<Option> autoT = new ObservableCollection<Option> { zh, zhTW, en, ja, ko, fr, es, it, de, tr, ru, pt, vi, id, th, ms, ar, hi };
            ObservableCollection<Option> zhT = new ObservableCollection<Option> { en, ja, ko, fr, es, it, de, tr, ru, pt, vi, id, th, ms };
            ObservableCollection<Option> zhTWT = new ObservableCollection<Option> {en, ja, ko, fr, es, it, de, tr, ru, pt, vi, id, th, ms };
            ObservableCollection<Option> enT = new ObservableCollection<Option> { zh, ja, ko, fr, es, it, de, tr, ru, pt, vi, id, th, ms, ar, hi };
            ObservableCollection<Option> jaT = new ObservableCollection<Option> { zh, en, ko };
            ObservableCollection<Option> koT = new ObservableCollection<Option> { zh, en, ja };
            ObservableCollection<Option> frT = new ObservableCollection<Option> { zh, en, es, it, de, tr, ru, pt };
            ObservableCollection<Option> esT = new ObservableCollection<Option> { zh, en, fr, it, de, tr, ru, pt };
            ObservableCollection<Option> itT = new ObservableCollection<Option> { zh, en, fr, es, de, tr, ru, pt };
            ObservableCollection<Option> deT = new ObservableCollection<Option> { zh, en, fr, es, it, tr, ru, pt };
            ObservableCollection<Option> trT = new ObservableCollection<Option> { zh, en, fr, es, it, de, ru, pt };
            ObservableCollection<Option> ruT = new ObservableCollection<Option> { zh, en, fr, es, it, de, tr, pt };
            ObservableCollection<Option> ptT = new ObservableCollection<Option> { zh, en, fr, es, it, de, tr, ru };
            ObservableCollection<Option> viT = new ObservableCollection<Option> { zh, en };
            ObservableCollection<Option> idT = new ObservableCollection<Option> { zh, en };
            ObservableCollection<Option> thT = new ObservableCollection<Option> { zh, en };
            ObservableCollection<Option> msT = new ObservableCollection<Option> { zh, en };
            ObservableCollection<Option> arT = new ObservableCollection<Option> { en };
            ObservableCollection<Option> hiT = new ObservableCollection<Option> { en };
            target.Add("auto", autoT);
            target.Add("zh", zhT);
            target.Add("zh-TW", zhTWT);
            target.Add("en", enT);
            target.Add("ja", jaT);
            target.Add("ko", koT);
            target.Add("fr", frT);
            target.Add("es", esT);
            target.Add("it", itT);
            target.Add("de", deT);
            target.Add("tr", trT);
            target.Add("ru", ruT);
            target.Add("pt", ptT);
            target.Add("vi", viT);
            target.Add("id", idT);
            target.Add("th", thT);
            target.Add("ms", msT);
            target.Add("ar", arT);
            target.Add("hi", hiT);
            return target;
        }
    }
}
