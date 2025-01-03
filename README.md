# Security - ReDoS (Regular Expression Denial of Service) Attack

## What is ReDoS?

ReDoS (Regular Expression Denial of Service) is a type of attack that exploits performance vulnerabilities in poorly designed regular expressions. It aims to significantly slow down or crash systems that process these regular expressions, resulting in a denial of service.


### How does ReDoS work?

ReDoS occurs when a regular expression with **exponential behavior** or **catastrophic backtracking** is evaluated against an input specifically crafted to trigger the worst-case performance. Instead of processing quickly, the regex engine can enter a prolonged loop, consuming significant computational resources.


### Example:

**Vulnerable Regular Expression:**
```regex
(a+)+$
```

**Malicious Input:**
```plaintext
aaaaaaaaaaaaaaaaaaaaa!
```
A long sequence of "a"s followed by something that doesn't match

In this example:
- The regex engine tries multiple combinations to match the pattern `a+` with the input.
- When the match fails, it backtracks to recheck previous combinations, which is computationally expensive.


## Impact of ReDoS
- **High CPU Usage**: Can slow down or overload servers.
- **Denial of Service**: Affects systems relying on regular expressions, such as APIs, input validation, and firewalls.
- **Reduced Scalability**: Exposed systems become more vulnerable under heavy load.


## How to Prevent ReDoS
1. **Avoid Vulnerable Patterns**: Rewrite regular expressions to eliminate structures prone to excessive backtracking.
   - Use non-greedy quantifiers (`*?`, `+?`) or safer alternatives.
2. **Input Validation**: Limit the size of inputs processed by regex.
3. **Use Safe Libraries**: Opt for regex libraries or engines that don't rely on backtracking.
4. **Performance Testing**: Test regular expressions with malicious inputs to identify problematic patterns.
5. **Monitoring and Mitigation**: Monitor resource usage and implement timeouts for regex operations.

ReDoS is a common issue, especially in languages and frameworks that use backtracking-based regex engines, such as c#, JavaScript, Python, and Java. Careful design of regular expressions is crucial to avoid vulnerabilities in critical systems.

### Example of timout for regex in c#
```csharp

public static class Validator
{
    public static bool IsValidEmail(this string email)
        => Regex.IsMatch(
            email,
            @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$",
            RegexOptions.None,
            TimeSpan.FromMilliseconds(500));
}
```
All regex operations should include a timeout to prevent ReDoS attacks caused by unmanageable inputs. However, in the previous example, the regex still relies on backtracking (which has exponential time complexity) and could be optimized for better performance.

### Example of a better regex for email validation

Regex with linear time complexity
```csharp
public static class Validator
{
    public static bool IsValidEmail(this string email)
        => Regex.IsMatch(
            email,
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.None,
            TimeSpan.FromMilliseconds(500));
}
```


## References
- [OWASP - Regular Expression Denial of Service (ReDoS)](https://owasp.org/www-community/attacks/Regular_expression_Denial_of_Service_-_ReDoS)
- [Wikipedia - ReDoS](https://en.wikipedia.org/wiki/ReDoS)
- [Tools - ReDoS](https://devina.io/redos-checker/)
