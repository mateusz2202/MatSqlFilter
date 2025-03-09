using System.Text;

namespace MatSqlFilter;

public static class SqlFilter
{
    public static (bool, string) GenerateFiltr(Filter filter)
        => GenerateRecursive(filter);

    private static (bool, string) GenerateRecursive(Filter filter)
    {
        var result = new StringBuilder();
        var conditions = new List<string>();

        foreach (var component in filter.Filters)
        {
            if (component is Condition condition)
            {
                if (!_filterConditions.ContainsKey(condition.Type))
                    return (false, "Invalid operator");

                conditions.Add(_filterConditions[condition.Type](condition));
            }
            else if (component is Filter subFilter)
            {
                var (isValid, subQuery) = GenerateRecursive(subFilter);
                if (!isValid) return (false, subQuery);
                conditions.Add("(" + subQuery + ")");
            }
        }

        if (conditions.Count > 1 && (!Enum.TryParse(filter.LogicOperator, true, out LogicOperator _)))
            return (false, "Invalid or missing logic operator");

        return (true, string.Join($" {filter.LogicOperator} ", conditions));
    }

    private static readonly Dictionary<string, Func<Condition, string>> _filterConditions = new()
    {
        { nameof(FilterType.Equals), f => $"{f.Column} = '{f.Value}'" },
        { nameof(FilterType.NotEquals), f => $"{f.Column} <> '{f.Value}'" },
        { nameof(FilterType.Like), f => $"{f.Column} LIKE '%{f.Value}%'" },
        { nameof(FilterType.Contains), f => $"{f.Column} IN ({f.Value})" },
        { nameof(FilterType.GreaterThan), f => $"{f.Column} > '{f.Value}'" },
        { nameof(FilterType.LessThan), f => $"{f.Column} < '{f.Value}'" },
    };

}


public enum FilterType
{
    Equals,
    NotEquals,
    Like,
    Contains,
    GreaterThan,
    LessThan
}


public enum LogicOperator
{
    And,
    Or
}


public interface IFilterComponent { }

public record Condition(string Column, string Value, string Type) : IFilterComponent;

public record Filter(List<IFilterComponent> Filters, string LogicOperator = null) : IFilterComponent;