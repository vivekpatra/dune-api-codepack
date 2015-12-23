# error\_kind #
## Summary ##

`error_kind` indicates the error type when [command\_status](CommandStatus.md) is _failed_.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="error_kind" value="{value}"/>
  ...
</command_result>
```


## Values ##

| **Value**            | **Summary**                                                                                                                                 |
|:---------------------|:--------------------------------------------------------------------------------------------------------------------------------------------|
| unknown\_command     | The specified command could not be recognized, Possibly because the command was introduced in a later version than the installed version.   |
| invalid\_parameters  | One or more parameters are missing, or there are invalid parameter values.                                                                  |
| illegal\_state       | The command does not apply to the current playback state.                                                                                   |
| operation\_failed    | The command was accepted, but an error occurred during command execution.                                                                   |
| internal\_error      | See the error description for more information.                                                                                             |