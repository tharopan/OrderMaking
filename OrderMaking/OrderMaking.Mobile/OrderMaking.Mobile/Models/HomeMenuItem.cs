using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMaking.Mobile.Models
{
    public enum MenuItemType
    {
        ScanOrder,
        GenerateList
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
