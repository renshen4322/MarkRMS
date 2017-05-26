using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
    public class SchemeResponseEntity
    {
        public List<SchemeModel> data { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int count { get; set; }
    }
    public class SchemeRoot
    {
        public SchemeModel Data { get; set; }
    }

    public class SchemePic
    {
        public List<string> screenshots { get; set; }

        public List<PsImg> p360s { get; set; }

        public AutoSave autosave { get; set; }
    }

    public class AutoSave {

        public string screenshot { get; set; }

        public string p360 { get; set; }

    }

    public class PsImg
    {
        public string campos { get; set; }

        public string p360 { get; set; }
    }

    public class SchemeModel
    {
        public SchemeModel()
        {
            schemePic = new SchemePic();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// DesignSchemeId
        /// </summary>
        public int designSchemeId { get; set; }
        /// <summary>
        /// da_piao_you_1
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// BuildMap
        /// </summary>
        public string sceneName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }

        /// </summary>
        public SchemePic schemePic { get; set; }

        public string images { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        public int revision { get; set; }
        /// <summary>
        /// Published
        /// </summary>
        public bool published { get; set; }
        /// <summary>
        /// Latest
        /// </summary>
        public bool latest { get; set; }
        /// <summary>
        /// 2017-01-24T02:45:24Z
        /// </summary>
        public string createdUtc { get; set; }
        /// <summary>
        /// Listable
        /// </summary>
        public bool listable { get; set; }
        /// <summary>
        /// Freezed
        /// </summary>
        public bool freezed { get; set; }
        /// <summary>
        /// IsLiked
        /// </summary>
        public bool isLiked { get; set; }
        /// <summary>
        /// LikedNumber
        /// </summary>
        public int likedNumber { get; set; }
        /// <summary>
        /// IsCollected
        /// </summary>
        public bool isCollected { get; set; }
        /// <summary>
        /// IsShared
        /// </summary>
        public bool isShared { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string colors { get; set; }
        /// <summary>
        /// CollectedNumber
        /// </summary>
        public int collectedNumber { get; set; }
    }
}
