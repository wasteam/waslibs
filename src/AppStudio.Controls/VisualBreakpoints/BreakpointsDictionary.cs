using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Controls
{
    internal class BreakpointsDictionary : Dictionary<string, BreakpointsTable>
    {
        public void Merge(BreakpointItemConfig breakpointConfig)
        {
            if (!base.ContainsKey(breakpointConfig.maxwidth))
            {
                base.Add(breakpointConfig.maxwidth, new BreakpointsTable());
            }
            base[breakpointConfig.maxwidth].Merge(breakpointConfig);
        }
    }

    internal class BreakpointsTable : List<BreakpointsItem>
    {
        public void Merge(BreakpointItemConfig breakpointConfig)
        {
            //this is because: System.Reflection.MemberInfo.get_MetadataToken()' cannot be used on the current platform. See http://go.microsoft.com/fwlink/?LinkId=248273 for more information.
            var jO = breakpointConfig.properties as JObject;
            var items = jO.Children()
                                .Cast<JProperty>()
                                .Select(p => new BreakpointsItem(p.Name, p.Value.ToString()))
                                .ToList();

            Merge(items);
        }


        public void Merge(IEnumerable<BreakpointsItem> value)
        {
            foreach (var item in value)
            {
                var existingItem = this.FirstOrDefault(v => v.Name == item.Name);
                if (existingItem == null)
                {
                    this.Add(item);
                }
                else
                {
                    existingItem.Value = item.Value;
                }
            }
        }
    }

    internal class BreakpointsItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public BreakpointsItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
