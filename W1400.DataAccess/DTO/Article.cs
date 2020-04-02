using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DTO
{
  public  class Article
  {
      public int STT { get; set; }
      public int ArticleID { get; set; }
      public string Title { get; set; }
      public int MenuID { get; set; }
      public string Description { get; set; }
      public string Detail { get; set; }
      public string Image { get; set; }
      public string ImageDetail { get; set; }
      public string ImageHot { get; set; }
      public string MetaData { get; set; }
      public string Tags { get; set; }
      public DateTime PublishDate { get; set; }
      public bool isHot { get; set; }
      public int Status { get; set; }
      public DateTime CreateDate { get; set; }
      public string CreateUser { get; set; }
      public DateTime ConfirmDate { get; set; }
      public string ConfirmUser { get; set; }
      public int CountView { get; set; }
      public string MenuName { get; set; }
      public string BottomDesc { get; set; }
      public int OrderHot { get; set; }

      public string Author { get; set; }
    }
  public class Menu
  {
      public int MenuID { get; set; }
      public string MenuName { get; set; }
      public string MenuDesc { get; set; }
      public int FatherID { get; set; }
      public string FatherName { get; set; }
      public int Status { get; set; }
      public string CreateUser { get; set; }
      public DateTime CreateDate { get; set; }
      public string Url { get; set; }
      public string UrlRedirect { get; set; }
      public string UrlFather { get; set; }
      public string UrlRedirectFather { get; set; }
  }
}
