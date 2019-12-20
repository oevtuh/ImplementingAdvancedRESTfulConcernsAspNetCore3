using System.Collections.Generic;

namespace CourseLibrary.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        bool ValidMappingExistFor<TSource, TDestionation>(string fileds);
    }
}