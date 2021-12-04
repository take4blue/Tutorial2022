using System;

namespace xamarin1.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public bool IsBeingDraggedOver { get; set; }
        public bool IsBeingDragged { get; set; }
    }
}