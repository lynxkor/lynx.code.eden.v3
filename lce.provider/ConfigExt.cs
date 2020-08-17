/* file name：lce.ext.providers.ConfigExt.cs
* author：lynx lynx.kor@163.com @ 2019/11/21 13:04:32
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for ConfigExt
* revision：
*
*/

namespace lce.provider
{
    /// <summary>
    /// action：ConfigExt
    /// </summary>
    public static class ConfigExt
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key">配置项</param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(key);
        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="key">  配置项</param>
        /// <param name="value">值</param>
        public static void Set(string key, string value)
        {
            try
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            catch (System.Configuration.ConfigurationErrorsException ex)
            {
                LogExt.e("设置配置信息出错", ex);
            }
        }
    }
}