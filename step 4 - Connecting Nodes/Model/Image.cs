using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Crunchbase.Model
{
    /*
     * {"available_sizes":
    [[[150,
       71],
      "assets/images/resized/0010/3529/103529v1-max-150x150.png"],
     [[151,
       72],
      "assets/images/resized/0010/3529/103529v1-max-250x250.png"],
     [[151,
       72],
      "assets/images/resized/0010/3529/103529v1-max-450x450.png"]],
   "attribution": null},
     */

    public class ImageSize: List<object>
    {
        public int? SizeX { 
            get 
            {
                return ((this[0] as object[])[0] as int?);
            }
        }

        public int? SizeY
        {
            get
            {
                return ((this[0] as object[])[1] as int?);
            }
        }

        public String Path {
            get
            {
                return (this[1] as String);
            }
        }
    }

    public class Image
    {
        public List<ImageSize> available_sizes;
        public String attribution { get; set; }

    }
}
