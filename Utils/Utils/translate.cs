using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace Utils
{
    
    class translate
    {
        private Config config;
        public translate(Config con)
        {
            config = con;
        }
        public string translateText(string origin)
        {
            Credential cred = new Credential
            {
                SecretId = config.getConfig(Config.SECRET_ID),
                SecretKey = config.getConfig(Config.SECRET_KEY)
            };
            TmtClient client = new TmtClient(cred, "ap-beijing");
            TextTranslateRequest request = new TextTranslateRequest();
            request.SourceText = origin;
            request.Source = config.getConfig(Config.SOURCE);
            request.Target = config.getConfig(Config.TARGET);
            request.UntranslatedText = "";
            string result;
            try
            {
                request.ProjectId = long.Parse(config.getConfig(Config.PROJECT_ID));
                TextTranslateResponse resp = client.TextTranslateSync(request);
                result = resp.TargetText;
            }
            catch (Exception e)
            {
                result = e.ToString();
            }
            return result;
        }
        public Tuple<string,string> translateImage(string data)
        {
            Credential cred = new Credential
            {
                SecretId = config.getConfig(Config.SECRET_ID),
                SecretKey = config.getConfig(Config.SECRET_KEY)
            };
            TmtClient client = new TmtClient(cred, "ap-beijing");
            ImageTranslateRequest req = new ImageTranslateRequest();
            req.Data = data;
            if (data.Length < 10) {
                return new Tuple<string, string> ("data过短："+data, "data过短：" + data);
            }
            req.Scene = "doc";
            req.SessionUuid = "session" + data.Substring(0,5);
            req.Source = config.getConfig(Config.SOURCE);
            req.Target = config.getConfig(Config.TARGET);
            string targetText = "";
            string sourceText = "";
            try
            {
                req.ProjectId = long.Parse(config.getConfig(Config.PROJECT_ID));
                ImageTranslateResponse resp = client.ImageTranslateSync(req);
                for (int i = 0; i < resp.ImageRecord.Value.Length; i++)
                {
                    sourceText += resp.ImageRecord.Value[i].SourceText + "\n";
                    targetText += resp.ImageRecord.Value[i].TargetText + "\n";
                }
            }
            catch (Exception e)
            {
                return new Tuple<string, string>(e.ToString(), e.ToString());
            }
            return new Tuple<string, string>(sourceText, targetText);
        }
    }
}
