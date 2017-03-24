using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock_Tester
{
    class Program
    {
        
        private static GroupTypeCache _groupType = null;
        private static RockContext _rockContext = null;
        private static Group _group = null;
        static void Main(string[] args)
        {
            _rockContext = new RockContext();
            var block = GroupTypeCache.GetFamilyGroupType();
            _groupType = block;

            var _IsFamilyGroupType = _groupType.Guid.Equals(Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid());


            var groupService = new GroupMemberService(_rockContext);

            var groups = groupService.Queryable(true)
                .Where(m => m.Group.GroupTypeId == _groupType.Id)
                .Select(m => m.Group)
                .Distinct()
                .OrderBy(m => m.Name)
                .ToList();

            foreach (var group in groups)
            {
                var count = group.Members.Select(x => x.Person).Where(x => x.RecordStatusValueId.Value == 3).Count();
                var memCount = group.Members.Select(x => x.Person).Where(x => x.ConnectionStatusValueId.Value == 146 || x.ConnectionStatusValueId.Value == 65).Count();
                if ((count != 0) && (memCount != 0))
                {
                    Console.WriteLine(group.Name);
                    Console.WriteLine(group.GroupLocations.Count);
                    foreach (var p in group.Members.Select(m => m.Person).OrderBy(a => a.NickName))
                    {
                        if (!p.IsDeceased)
                        {
                            Console.WriteLine("\t-{0} {1} {2} {3}", p.NickName, p.LastName, p.Age, p.Gender);
                        }
                    }
                    Console.WriteLine();
                }
                
            }
            Console.ReadKey();
        }
    }
}
