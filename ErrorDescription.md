# error\_description #
## Summary ##

`error_description` is a human-friendly error message which usually provides more insight than the [error\_kind](ErrorKind.md) parameter.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="error_description" value="{value}"/>
  ...
</command_result>
```

## Values ##

There is no fixed set of possible return values.

## Notes ##

If you want to localize the error messages, you can use regular expressions to parse the error description.