﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.Models;
using CourseLibrary.API.Entities;


namespace CourseLibrary.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                { "MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory" } )},
                { "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
                { "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }

        public bool ValidMappingExistFor<TSource, TDestionation>(string fileds)
        {
            var mapping = GetPropertyMapping<TSource, TDestionation>();

            if (string.IsNullOrEmpty(fileds))
            {
                return true;
            };

            var fieldsAfterSplit = fileds.Split(',');
            
            foreach (var field in fieldsAfterSplit)
            {                
                var trimmedField = field.Trim();
                
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!mapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingType = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingType.Count() == 1)
            {
                return matchingType.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                 $"for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
