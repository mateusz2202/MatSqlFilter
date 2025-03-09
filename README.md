# SqlFilter

## Overview
`SqlFilter` is a simple and flexible library that generates SQL `WHERE` clauses dynamically using conditions and logical operators. It supports nested filters and allows complex query building.

## Features
- Supports multiple condition types (`Equals`, `NotEquals`, `Like`, `Contains`, etc.)
- Allows logical operators (`AND`, `OR`)
- Supports nested filtering using recursion
- Generates SQL-friendly query strings

## Installation
This is a standalone C# class. Simply include `SqlFilter.cs` in your project.

## Usage
### Sample Code
```csharp
var c1 = new Condition("Name", "John,xd", nameof(FilterType.Contains));
var c2 = new Condition("Age", "25", nameof(FilterType.GreaterThan));
var c3 = new Condition("City", "New York", nameof(FilterType.Equals));
var c4 = new Condition("Country", "USA1", nameof(FilterType.Equals));
var c5 = new Condition("Country", "USA2", nameof(FilterType.Equals));
var c6 = new Condition("Country", "USA3", nameof(FilterType.Equals));
var c7 = new Condition("Country", "USA4", nameof(FilterType.Equals));
var c8 = new Condition("Country", "USA5", nameof(FilterType.NotEquals));

// Creating logical groups
var group1 = new Filter(new List<IFilterComponent> { c1, c2 }, "OR");
var group2 = new Filter(new List<IFilterComponent> { c3, c4 }, "AND");
var group3 = new Filter(new List<IFilterComponent> { c5, c6 }, "OR");
var group4 = new Filter(new List<IFilterComponent> { c7, c8 }, "AND");

// Nesting filters
var nestedFilter1 = new Filter(new List<IFilterComponent> { group1, group2 }, "AND");
var nestedFilter2 = new Filter(new List<IFilterComponent> { group3, group4 }, "OR");

var finalFilter = new Filter(new List<IFilterComponent> { nestedFilter1, nestedFilter2 }, "AND");

// Generating SQL WHERE clause
Console.WriteLine(SqlFilter.GenerateFiltr(finalFilter).Item2);
```

### Expected Output
```sql
((Name IN ('John,xd') OR Age > '25') AND (City = 'New York' AND Country = 'USA1')) AND ((Country = 'USA2' OR Country = 'USA3') OR (Country = 'USA4' AND Country <> 'USA5'))
```

## Filter Operators
- `Equals` → `Column = 'Value'`
- `NotEquals` → `Column <> 'Value'`
- `Like` → `Column LIKE '%Value%'`
- `Contains` → `Column IN (Value1, Value2, ...)`
- `GreaterThan` → `Column > 'Value'`
- `LessThan` → `Column < 'Value'`

## Logical Operators
- `AND`
- `OR`

## Notes
- Nested filters allow building complex `WHERE` conditions.
- Ensure that the logic operator is correctly set (either `AND` or `OR`).
- Avoid SQL injection by using parameterized queries when integrating this logic into a real database system.

## License
MIT License. Free to use and modify.

