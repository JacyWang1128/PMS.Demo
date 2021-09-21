using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Demo.Utils
{
    public class AForgeHelper
    {
        public FilterInfoCollection videoDevices = null;
        public VideoCaptureDevice videoDevice = null;

        public AForgeHelper()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        public List<string> GetCaptureList()
        {
            List<string> result = null;
            if (videoDevices.Count == 0)
                return result;
            result = new List<string>();
            foreach (FilterInfo item in videoDevices)
            {
                result.Add(item.Name);
            }
            return result;
        }

        public void SetDevice(int index)
        {
            videoDevice = null;
            videoDevice = new VideoCaptureDevice(videoDevices[index].MonikerString);
        }
    }
}
