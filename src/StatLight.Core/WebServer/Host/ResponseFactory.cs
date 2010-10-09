﻿using System;
using System.Globalization;
using System.Threading;
using StatLight.Core.Configuration;
using StatLight.Core.Properties;
using StatLight.Core.Serialization;

namespace StatLight.Core.WebServer.Host
{
    public class ResponseFactory
    {
        private readonly Func<byte[]> _xapToTestFactory;
        private readonly byte[] _hostXap;
        private readonly string _serializedConfiguration;

        public ResponseFactory(Func<byte[]> xapToTestFactory, byte[] hostXap, ClientTestRunConfiguration clientTestRunConfiguration)
        {
            _xapToTestFactory = xapToTestFactory;
            _hostXap = hostXap;
            _serializedConfiguration = clientTestRunConfiguration.Serialize();
        }

        private static int _htmlPageInstanceId = 0;

        public ResponseFile Get(string localPath)
        {
            if (IsKnown(localPath, StatLightServiceRestApi.CrossDomain))
                return new ResponseFile { FileData = Resources.CrossDomain.ToByteArray(), ContentType = "text/xml" };

            if (IsKnown(localPath, StatLightServiceRestApi.ClientAccessPolicy))
                return new ResponseFile { FileData = Resources.ClientAccessPolicy.ToByteArray(), ContentType = "text/xml" };

            if (IsKnown(localPath, StatLightServiceRestApi.GetHtmlTestPage))
            {
                return GetTestHtmlPage();
            }

            if (IsKnown(localPath, StatLightServiceRestApi.GetXapToTest))
                return new ResponseFile { FileData = _xapToTestFactory(), ContentType = "application/x-silverlight-app" };

            if (IsKnown(localPath, StatLightServiceRestApi.GetTestPageHostXap))
                return new ResponseFile { FileData = _hostXap, ContentType = "application/x-silverlight-app" };

            if (IsKnown(localPath, StatLightServiceRestApi.GetTestRunConfiguration))
                return new ResponseFile { FileData = _serializedConfiguration.ToByteArray(), ContentType = "text/xml" };

            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool IsKnownFile(string localPath)
        {
            if (IsKnown(localPath, StatLightServiceRestApi.CrossDomain))
                return true;

            if (IsKnown(localPath, StatLightServiceRestApi.ClientAccessPolicy))
                return true;

            if (IsKnown(localPath, StatLightServiceRestApi.GetHtmlTestPage))
                return true;

            if (IsKnown(localPath, StatLightServiceRestApi.GetXapToTest))
                return true;

            if (IsKnown(localPath, StatLightServiceRestApi.GetTestPageHostXap))
                return true;

            if (IsKnown(localPath, StatLightServiceRestApi.GetTestRunConfiguration))
                return true;

            return false;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static ResponseFile GetTestHtmlPage()
        {
            var page = Resources.TestPage.Replace("BB86D193-AD39-494A-AEB7-58F948BA5D93", _htmlPageInstanceId.ToString(CultureInfo.InvariantCulture));

            Interlocked.Increment(ref _htmlPageInstanceId);

            return new ResponseFile { FileData = page.ToByteArray(), ContentType = "text/html" };
        }

        private static bool IsKnown(string filea, string fileb)
        {
            return string.Equals(filea, fileb, StringComparison.OrdinalIgnoreCase);
        }

        public static void Reset()
        {
            _htmlPageInstanceId = 0;
        }
    }
}