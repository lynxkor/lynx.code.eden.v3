﻿// action：
// file name：${namespace}.SMSHelper.cs
// author：lynx lynx.kor@163.com @ 2019/6/7 15:27
// copyright (c) 2019 lynxce.com
// desc：
// > add description for SMSHelper
// revision：
//
using System;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;

namespace lce.provider
{
    public static class SMSHelper
    {

        public static void Send(string phones, object content, string signName = "壹途", string templateCode = "SMS_162730288")
        {
            IClientProfile profile = DefaultProfile.GetProfile("default", "osMT0gwEJx32AUom", "L9pIDvNwV56Rlq07oZDrFI3OaPLeXg");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = "dysmsapi.aliyuncs.com",
                Version = "2017-05-25",
                Action = "SendSms"
            };
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", phones);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", templateCode);
            request.AddQueryParameters("TemplateParam", content.ToJson());
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}