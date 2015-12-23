# command\_status #

## Summary ##

`command_status` indicates whether the command was successful or not, or if a timeout was reached before command execution completed.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="command_status" value="{value}"/>
  ...
</command_result>
```


## Values ##

| **Value**| **Summary**|
|:---------|:-----------|
|ok        |The command ran to completion.|
|failed    |The command ran to completion, but an error was encountered.|
|timeout   |Timeout was reached before command execution completed.|

## Notes ##

  * When the value is _failed_, see [error\_kind](ErrorKind.md) and [error\_description](ErrorDescription.md) for more details.

  * If you get frequent _timeout_ responses, consider increasing the value of the timeout parameter.