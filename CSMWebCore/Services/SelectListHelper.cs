using CSMWebCore.Entities;
using CSMWebCore.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public static class SelectListHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Event> events, int selectedId)
        {
            return
                events.OrderBy(e => e.Id)
                .Select(e => new SelectListItem
                {
                    Selected = (e.Id == selectedId),
                    Text = $"{e.Name}",
                    Value = e.Id.ToString()
                });
        }
    }
}
