<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    //RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

    var lines = AdventOfCode.GetLines("day_10.input");

    List<Result> results = new();
    foreach (var line in lines)
    {
        var tokenizer = new Tokenizer(new StringReader(line));
        var parser = new Parser(tokenizer);
        results.Add(parser.Parse());
    }

    GetScorePartOne(results.Where(x => !x.Valid)).Dump("Part One");
    GetScorePartTwo(results.Where(x => x.Valid)).Dump("Part Two");
}

private static long GetScorePartOne(IEnumerable<Result> results)
{
    return results.Select(x => PartOnePenalty[x.Token]).Sum();
}

private static long GetScorePartTwo(IEnumerable<Result> results)
{
    var scores = results
        .Select(x => GetScorePartTwo(x.Missing))
        .OrderBy(x => x)
        .ToList();

    return scores[scores.Count / 2];
}

private static long GetScorePartTwo(string missing)
{
    long score = 0;

    foreach (var c in missing)
    {
        score = score * 5 + PartTwoPenalty[c];
    }

    return score;
}

private static Dictionary<char, int> PartOnePenalty = new()
{
    [')'] = 3,
    [']'] = 57,
    ['}'] = 1197,
    ['>'] = 25137,
};

private static Dictionary<char, int> PartTwoPenalty = new()
{
    [')'] = 1,
    [']'] = 2,
    ['}'] = 3,
    ['>'] = 4,
};

public enum Token
{
    L_PAREN = '(',
    R_PAREN = ')',
    L_BRACE = '{',
    R_BRACE = '}',
    L_BRACKET = '[',
    R_BRACKET = ']',
    L_ANGLE = '<',
    R_ANGLE = '>'
}

public class Tokenizer
{
    private readonly TextReader _sr;
    private Token? _next;

    public Tokenizer(TextReader sr)
    {
        _sr = sr;
    }

    public Tokenizer(string line) : this(new StringReader(line)) { }

    public Token Peek()
    {
        var c = _sr.Read();
        _next = (Token)c;
        return _next.Value;
    }

    public bool HasNext()
    {
        return _sr.Peek() > -1;
    }

    public Token Pop()
    {
        if (!_next.HasValue)
        {
            return (Token)_sr.Read();
        }

        var result = _next.Value;
        _next = null;
        return result;
    }
}

public class Parser
{
    private readonly Tokenizer _tokenizer;

    public Parser(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    private Dictionary<Token, Token> pairs = new()
    {
        [Token.L_ANGLE] = Token.R_ANGLE,
        [Token.L_BRACE] = Token.R_BRACE,
        [Token.L_PAREN] = Token.R_PAREN,
        [Token.L_BRACKET] = Token.R_BRACKET
    };

    public Result Parse()
    {
        var missing = "";
        while (_tokenizer.HasNext())
        {
            var result = ParseInner();
            if (!result.Valid)
                return result;
            missing += result.Missing;
        }
        return Result.Ok(missing);
    }

    public Result ParseInner()
    {
        var token = _tokenizer.Pop();
        var missing = string.Empty;

        if (!pairs.ContainsKey(token))
        {
            return Result.Error(token);
        }

        while (_tokenizer.HasNext())
        {
            var next = _tokenizer.Peek();
            if (next == pairs[token])
            {
                _tokenizer.Pop();
                return Result.Ok(missing);
            }

            var nested = ParseInner();
            if (!nested.Valid)
            {
                return nested;
            }

            missing += nested.Missing;
        }

        return Result.Ok(missing + (char)pairs[token]);
    }
}

public class Result
{
    public bool Valid { get; set; }
    public char Token { get; set; }
    public string Missing { get; set; }

    public static Result Error(Token token)
    {
        return new Result { Token = (char)token, Valid = false };
    }

    public static Result Ok(string remaining)
    {
        return new Result { Valid = true, Missing = remaining };
    }
}

#region private::Tests

[Theory]
[InlineData("{()", "}")]
[InlineData("[({(<(())[]>[[{[]{<()<>>", "}}]])})]")]
[InlineData("[(()[<>])]({[<{<<[]>>(", ")}>]})")]
void ParseScope_WithIncompleteLine_ReturnsMissingParts(string input, string expected)
{
    var parser = new Parser(new Tokenizer(input));
    var result = parser.Parse();

    Assert.Equal(expected, result.Missing);
}

[Theory]
[InlineData("])}>", "294")]
void GetScorePartTwo_Always_ReturnsCorrectScore(string input, int expected)
{
    var result = GetScorePartTwo(input);

    Assert.Equal(expected, result);
}

#endregion
