# ir\_code (parameter) #
## Summary ##

`ir_code` indicates a key code used to emulate key presses.

## Values ##

The first column represents the key label.

The second column represents the key code (big endian notation).

The third column represents the little endian notation. **Use this as the parameter value**.

**NOTE**: spaces between bytes are only there to improve readability.


| **Key** | **NEC Code** (big endian) | **NEC Code** (little endian) | **Big remote** (1) | **Big remote** (2) | **Small remote** | **Remarks** |
|:--------|:--------------------------|:-----------------------------|:-------------------|:-------------------|:-----------------|:------------|
|Eject    |00 BF 10 EF                |EF 10 BF 00                   |✔                   |✔                   |                  |             |
|Mute     |00 BF 46 B9                |B9 46 BF 00                   |✔                   |✔                   |✔                 |             |
|Mode     |00 BF 45 BA                |BA 45 BF 00                   |✔                   |✔                   |                  |             |
|Power    |00 BF 43 BC                |BC 43 BF 00                   |✔                   |✔                   |✔                 |             |
|A (red)  |00 BF 40 BF                |BF 40 BF 00                   |✔                   |✔                   |✔                 |             |
|B (green)|00 BF 1F E0                |E0 1F BF 00                   |✔                   |✔                   |✔                 |             |
|C (yellow)|00 BF 00 FF                |FF 00 BF 00                   |✔                   |✔                   |✔                 |             |
|D (blue) |00 BF 41 BE                |BE 41 BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 0|00 BF 0A F5                |F5 0A BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 1|00 BF 0B F4                |F4 0B BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 2|00 BF 0C F3                |F3 0C BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 3|00 BF 0D F2                |F2 0D BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 4|00 BF 0E F1                |F1 0E BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 5|00 BF 0F F0                |F0 0F BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 6|00 BF 01 FE                |FE 01 BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 7|00 BF 11 EE                |EE 11 BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 8|00 BF 12 ED                |ED 12 BF 00                   |✔                   |✔                   |✔                 |             |
|Numeric 9|00 BF 13 EC                |EC 13 BF 00                   |✔                   |✔                   |✔                 |             |
|Clear    |00 BF 05 FA                |FA 05 BF 00                   |✔                   |✔                   |✔                 |             |
|Select   |00 BF 42 BD                |BD 42 BF 00                   |✔                   |✔                   |✔                 |             |
|Volume up|00 BF 52 AD                |AD 52 BF 00                   |✔                   |✔                   |                  |             |
|Volume down|00 BF 53 AC                |AC 53 BF 00                   |✔                   |✔                   |                  |             |
|Page up  |00 BF 4B B4                |B4 4B BF 00                   |✔                   |✔                   |                  |             |
|Page down|00 BF 4C B3                |B3 4C BF 00                   |✔                   |✔                   |                  |             |
|Search   |00 BF 06 F9                |F9 06 BF 00                   |✔                   |✔                   |                  |             |
|Zoom     |00 BF 02 FD                |FD 02 BF 00                   |✔                   |✔                   |                  |             |
|Setup    |00 BF 4E B1                |B1 4E BF 00                   |✔                   |✔                   |                  |             |
|Up       |00 BF 15 EA                |EA 15 BF 00                   |✔                   |✔                   |✔                 |             |
|Down     |00 BF 16 E9                |E9 16 BF 00                   |✔                   |✔                   |✔                 |             |
|Left     |00 BF 17 E8                |E8 17 BF 00                   |✔                   |✔                   |✔                 |             |
|Right    |00 BF 18 E7                |E7 18 BF 00                   |✔                   |✔                   |✔                 |             |
|Enter    |00 BF 14 EB                |EB 14 BF 00                   |✔                   |✔                   |✔                 |             |
|Return   |00 BF 04 FB                |FB 04 BF 00                   |✔                   |✔                   |✔                 |             |
|Info     |00 BF 50 AF                |AF 50 BF 00                   |✔                   |✔                   |✔                 |             |
|Pop up menu|00 BF 07 F8                |F8 07 BF 00                   |✔                   |✔                   |✔                 |             |
|Top menu |00 BF 51 AE                |AE 51 BF 00                   |✔                   |✔                   |✔                 |             |
|Play     |00 BF 48 B7                |B7 48 BF 00                   |✔                   |✔                   |                  |             |
|Pause    |00 BF 1E E1                |E1 1E BF 00                   |✔                   |✔                   |                  |             |
|Play/Pause|00 BF 56 A9                |A9 56 BF 00                   |                    |                    |✔                 |             |
|Previous |00 BF 49 B6                |B6 49 BF 00                   |✔                   |✔                   |✔                 |             |
|Next     |00 BF 1D E2                |E2 1D BF 00                   |✔                   |✔                   |✔                 |             |
|Stop     |00 BF 19 E6                |E6 19 BF 00                   |✔                   |✔                   |✔                 |             |
|Slow     |00 BF 1A E5                |E5 1A BF 00                   |✔                   |✔                   |✔                 |             |
|Rewind   |00 BF 1C E3                |E3 1C BF 00                   |✔                   |✔                   |                  |             |
|Forward  |00 BF 1B E4                |E4 1B BF 00                   |✔                   |✔                   |✔                 |             |
|Subtitle |00 BF 54 AB                |AB 54 BF 00                   |✔                   |✔                   |✔                 |             |
|Angle/Rotate|00 BF 4D B2                |B2 4D BF 00                   |✔                   |✔                   |✔                 |             |
|Audio    |00 BF 44 BB                |BB 44 BF 00                   |✔                   |✔                   |✔                 |             |
|Repeat   |00 BF 4F B0                |B0 4F BF 00                   |✔                   |                    |                  |             |
|REC      |00 BF 60 9F                |9F 60 BF 00                   |                    |✔                   |                  |             |
|REC      |00 BF 55 AA                |AA 55 BF 00                   |                    |                    |✔                 |             |
|Shuffle/PIP|00 BF 47 B8                |B8 47 BF 00                   |✔                   |                    |                  |             |
|Dune     |00 BF 61 9E                |9E 61 BF 00                   |                    |✔                   |                  |             |
|Top menu/Dune|00 BF 57 A8                |A8 57 BF 00                   |                    |                    |✔                 |             |
|URL/2nd audio|00 BF 03 FC                |FC 03 BF 00                   |✔                   |                    |                  |             |
|URL      |00 BF 62 9D                |9D 62 BF 00                   |                    |✔                   |                  |             |
|Discrete power on|00 BF 5F A0                |A0 5F BF 00                   |                    |                    |                  |Discrete power on is ignored if the player is already turned on; otherwise it is powered on.|
|Discrete power off|00 BF 5E A1                |A1 5E BF 00                   |                    |                    |                  |Discrete power off is ignored if the player is already turned off; otherwise it is powered off.|
|Power    |00 BF 70 8F                |8F 70 BF 00                   |                    |                    |                  |Toggles between power on/off.|
|Power    |00 BF 71 8E                |8E 71 BF 00                   |                    |                    |                  |First press: standby, consecutive press: power off.|

**Big remote** (version 1): Shipped with Dune HD Base 2.0, Dune HD Prime/Base 3.0, Dune HD Smart D1/H1/B1, Dune HD Max, Dune HD Duo, Dune HD Lite 53D.

**Big remote** (version 2): Shipped with Dune HD Smart D1/H1/B1, Dune HD Max, Dune HD Duo, Dune HD Lite 53D.

**Small remote**: Shipped with Dune HD TV-101/301.

Not every key is present on every shipped remote; some keys can only be used by programmable remotes.

## Unmapped ##

Because the NEC protocol allows up to 255 unique keys, some code combinations do not (currently) have a use.

| **Key** | **NEC Code** (big endian) | **NEC Code** (little endian) |
|:--------|:--------------------------|:-----------------------------|
|Reserved |00 BF 08 F7                |F7 08 BF 00                   |
|Reserved |00 BF 09 F6                |F6 09 BF 00                   |
|Reserved |00 BF 20 DF                |DF 20 BF 00                   |
|Reserved |00 BF 21 DE                |DE 21 BF 00                   |
|Reserved |00 BF 22 DD                |DD 22 BF 00                   |
|Reserved |00 BF 23 DC                |DC 23 BF 00                   |
|Reserved |00 BF 24 DB                |DB 24 BF 00                   |
|Reserved |00 BF 25 DA                |DA 25 BF 00                   |
|Reserved |00 BF 26 D9                |D9 26 BF 00                   |
|Reserved |00 BF 27 D8                |D8 27 BF 00                   |
|Reserved |00 BF 28 D7                |D7 28 BF 00                   |
|Reserved |00 BF 29 D6                |D6 29 BF 00                   |
|Reserved |00 BF 2A D5                |D5 2A BF 00                   |
|Reserved |00 BF 2B D4                |D4 2B BF 00                   |
|Reserved |00 BF 2C D3                |D3 2C BF 00                   |
|Reserved |00 BF 2D D2                |D2 2D BF 00                   |
|Reserved |00 BF 2E D1                |D1 2E BF 00                   |
|Reserved |00 BF 2F D0                |D0 2F BF 00                   |
|Reserved |00 BF 30 CF                |CF 30 BF 00                   |
|Reserved |00 BF 31 CE                |CE 31 BF 00                   |
|Reserved |00 BF 32 CD                |CD 32 BF 00                   |
|Reserved |00 BF 33 CC                |CC 33 BF 00                   |
|Reserved |00 BF 34 CB                |CB 34 BF 00                   |
|Reserved |00 BF 35 CA                |CA 35 BF 00                   |
|Reserved |00 BF 36 C9                |C9 36 BF 00                   |
|Reserved |00 BF 37 C8                |C8 37 BF 00                   |
|Reserved |00 BF 38 C7                |C7 38 BF 00                   |
|Reserved |00 BF 39 C6                |C6 39 BF 00                   |
|Reserved |00 BF 3A C5                |C5 3A BF 00                   |
|Reserved |00 BF 3B C4                |C4 3B BF 00                   |
|Reserved |00 BF 3C C3                |C3 3C BF 00                   |
|Reserved |00 BF 3D C2                |C2 3D BF 00                   |
|Reserved |00 BF 3E C1                |C1 3E BF 00                   |
|Reserved |00 BF 3F C0                |C0 3F BF 00                   |
|Reserved |00 BF 4A B5                |B5 4A BF 00                   |
|Reserved |00 BF 58 A7                |A7 58 BF 00                   |
|Reserved |00 BF 59 A6                |A6 59 BF 00                   |
|Reserved |00 BF 5A A5                |A5 5A BF 00                   |
|Reserved |00 BF 5B A4                |A4 5B BF 00                   |
|Reserved |00 BF 5C A3                |A3 5C BF 00                   |
|Reserved |00 BF 5D A2                |A2 5D BF 00                   |
|Reserved |00 BF 5E A1                |A1 5E BF 00                   |
|Reserved |00 BF 5F A0                |A0 5F BF 00                   |
|Reserved |00 BF 63 9C                |9C 63 BF 00                   |
|Reserved |00 BF 64 9B                |9B 64 BF 00                   |
|Reserved |00 BF 65 9A                |9A 65 BF 00                   |
|Reserved |00 BF 66 99                |99 66 BF 00                   |
|Reserved |00 BF 67 98                |98 67 BF 00                   |
|Reserved |00 BF 68 97                |97 68 BF 00                   |
|Reserved |00 BF 69 96                |96 69 BF 00                   |
|Reserved |00 BF 6A 95                |95 6A BF 00                   |
|Reserved |00 BF 6B 94                |94 6B BF 00                   |
|Reserved |00 BF 6C 93                |93 6C BF 00                   |
|Reserved |00 BF 6D 92                |92 6D BF 00                   |
|Reserved |00 BF 6E 91                |91 6E BF 00                   |
|Reserved |00 BF 6F 90                |90 6F BF 00                   |
|Reserved |00 BF 72 8D                |8D 72 BF 00                   |
|Reserved |00 BF 73 8C                |8C 73 BF 00                   |
|Reserved |00 BF 74 8B                |8B 74 BF 00                   |
|Reserved |00 BF 75 8A                |8A 75 BF 00                   |
|Reserved |00 BF 76 89                |89 76 BF 00                   |
|Reserved |00 BF 77 88                |88 77 BF 00                   |
|Reserved |00 BF 78 87                |87 78 BF 00                   |
|Reserved |00 BF 79 86                |86 79 BF 00                   |
|Reserved |00 BF 7A 85                |85 7A BF 00                   |
|Reserved |00 BF 7B 84                |84 7B BF 00                   |
|Reserved |00 BF 7C 83                |83 7C BF 00                   |
|Reserved |00 BF 7D 82                |82 7D BF 00                   |
|Reserved |00 BF 7E 81                |81 7E BF 00                   |
|Reserved |00 BF 7F 80                |80 7F BF 00                   |
|Reserved |00 BF 80 7F                |7F 80 BF 00                   |
|Reserved |00 BF 81 7E                |7E 81 BF 00                   |
|Reserved |00 BF 82 7D                |7D 82 BF 00                   |
|Reserved |00 BF 83 7C                |7C 83 BF 00                   |
|Reserved |00 BF 84 7B                |7B 84 BF 00                   |
|Reserved |00 BF 85 7A                |7A 85 BF 00                   |
|Reserved |00 BF 86 79                |79 86 BF 00                   |
|Reserved |00 BF 87 78                |78 87 BF 00                   |
|Reserved |00 BF 88 77                |77 88 BF 00                   |
|Reserved |00 BF 89 76                |76 89 BF 00                   |
|Reserved |00 BF 8A 75                |75 8A BF 00                   |
|Reserved |00 BF 8B 74                |74 8B BF 00                   |
|Reserved |00 BF 8C 73                |73 8C BF 00                   |
|Reserved |00 BF 8D 72                |72 8D BF 00                   |
|Reserved |00 BF 8E 71                |71 8E BF 00                   |
|Reserved |00 BF 8F 70                |70 8F BF 00                   |
|Reserved |00 BF 90 6F                |6F 90 BF 00                   |
|Reserved |00 BF 91 6E                |6E 91 BF 00                   |
|Reserved |00 BF 92 6D                |6D 92 BF 00                   |
|Reserved |00 BF 93 6C                |6C 93 BF 00                   |
|Reserved |00 BF 94 6B                |6B 94 BF 00                   |
|Reserved |00 BF 95 6A                |6A 95 BF 00                   |
|Reserved |00 BF 96 69                |69 96 BF 00                   |
|Reserved |00 BF 97 68                |68 97 BF 00                   |
|Reserved |00 BF 98 67                |67 98 BF 00                   |
|Reserved |00 BF 99 66                |66 99 BF 00                   |
|Reserved |00 BF 9A 65                |65 9A BF 00                   |
|Reserved |00 BF 9B 64                |64 9B BF 00                   |
|Reserved |00 BF 9C 63                |63 9C BF 00                   |
|Reserved |00 BF 9D 62                |62 9D BF 00                   |
|Reserved |00 BF 9E 61                |61 9E BF 00                   |
|Reserved |00 BF 9F 60                |60 9F BF 00                   |
|Reserved |00 BF A0 5F                |5F A0 BF 00                   |
|Reserved |00 BF A1 5E                |5E A1 BF 00                   |
|Reserved |00 BF A2 5D                |5D A2 BF 00                   |
|Reserved |00 BF A3 5C                |5C A3 BF 00                   |
|Reserved |00 BF A4 5B                |5B A4 BF 00                   |
|Reserved |00 BF A5 5A                |5A A5 BF 00                   |
|Reserved |00 BF A6 59                |59 A6 BF 00                   |
|Reserved |00 BF A7 58                |58 A7 BF 00                   |
|Reserved |00 BF A8 57                |57 A8 BF 00                   |
|Reserved |00 BF A9 56                |56 A9 BF 00                   |
|Reserved |00 BF AA 55                |55 AA BF 00                   |
|Reserved |00 BF AB 54                |54 AB BF 00                   |
|Reserved |00 BF AC 53                |53 AC BF 00                   |
|Reserved |00 BF AD 52                |52 AD BF 00                   |
|Reserved |00 BF AE 51                |51 AE BF 00                   |
|Reserved |00 BF AF 50                |50 AF BF 00                   |
|Reserved |00 BF B0 4F                |4F B0 BF 00                   |
|Reserved |00 BF B1 4E                |4E B1 BF 00                   |
|Reserved |00 BF B2 4D                |4D B2 BF 00                   |
|Reserved |00 BF B3 4C                |4C B3 BF 00                   |
|Reserved |00 BF B4 4B                |4B B4 BF 00                   |
|Reserved |00 BF B5 4A                |4A B5 BF 00                   |
|Reserved |00 BF B6 49                |49 B6 BF 00                   |
|Reserved |00 BF B7 48                |48 B7 BF 00                   |
|Reserved |00 BF B8 47                |47 B8 BF 00                   |
|Reserved |00 BF B9 46                |46 B9 BF 00                   |
|Reserved |00 BF BA 45                |45 BA BF 00                   |
|Reserved |00 BF BB 44                |44 BB BF 00                   |
|Reserved |00 BF BC 43                |43 BC BF 00                   |
|Reserved |00 BF BD 42                |42 BD BF 00                   |
|Reserved |00 BF BE 41                |41 BE BF 00                   |
|Reserved |00 BF BF 40                |40 BF BF 00                   |
|Reserved |00 BF C0 3F                |3F C0 BF 00                   |
|Reserved |00 BF C1 3E                |3E C1 BF 00                   |
|Reserved |00 BF C2 3D                |3D C2 BF 00                   |
|Reserved |00 BF C3 3C                |3C C3 BF 00                   |
|Reserved |00 BF C4 3B                |3B C4 BF 00                   |
|Reserved |00 BF C5 3A                |3A C5 BF 00                   |
|Reserved |00 BF C6 39                |39 C6 BF 00                   |
|Reserved |00 BF C7 38                |38 C7 BF 00                   |
|Reserved |00 BF C8 37                |37 C8 BF 00                   |
|Reserved |00 BF C9 36                |36 C9 BF 00                   |
|Reserved |00 BF CA 35                |35 CA BF 00                   |
|Reserved |00 BF CB 34                |34 CB BF 00                   |
|Reserved |00 BF CC 33                |33 CC BF 00                   |
|Reserved |00 BF CD 32                |32 CD BF 00                   |
|Reserved |00 BF CE 31                |31 CE BF 00                   |
|Reserved |00 BF CF 30                |30 CF BF 00                   |
|Reserved |00 BF D0 2F                |2F D0 BF 00                   |
|Reserved |00 BF D1 2E                |2E D1 BF 00                   |
|Reserved |00 BF D2 2D                |2D D2 BF 00                   |
|Reserved |00 BF D3 2C                |2C D3 BF 00                   |
|Reserved |00 BF D4 2B                |2B D4 BF 00                   |
|Reserved |00 BF D5 2A                |2A D5 BF 00                   |
|Reserved |00 BF D6 29                |29 D6 BF 00                   |
|Reserved |00 BF D7 28                |28 D7 BF 00                   |
|Reserved |00 BF D8 27                |27 D8 BF 00                   |
|Reserved |00 BF D9 26                |26 D9 BF 00                   |
|Reserved |00 BF DA 25                |25 DA BF 00                   |
|Reserved |00 BF DB 24                |24 DB BF 00                   |
|Reserved |00 BF DC 23                |23 DC BF 00                   |
|Reserved |00 BF DD 22                |22 DD BF 00                   |
|Reserved |00 BF DE 21                |21 DE BF 00                   |
|Reserved |00 BF DF 20                |20 DF BF 00                   |
|Reserved |00 BF E0 1F                |1F E0 BF 00                   |
|Reserved |00 BF E1 1E                |1E E1 BF 00                   |
|Reserved |00 BF E2 1D                |1D E2 BF 00                   |
|Reserved |00 BF E3 1C                |1C E3 BF 00                   |
|Reserved |00 BF E4 1B                |1B E4 BF 00                   |
|Reserved |00 BF E5 1A                |1A E5 BF 00                   |
|Reserved |00 BF E6 19                |19 E6 BF 00                   |
|Reserved |00 BF E7 18                |18 E7 BF 00                   |
|Reserved |00 BF E8 17                |17 E8 BF 00                   |
|Reserved |00 BF E9 16                |16 E9 BF 00                   |
|Reserved |00 BF EA 15                |15 EA BF 00                   |
|Reserved |00 BF EB 14                |14 EB BF 00                   |
|Reserved |00 BF EC 13                |13 EC BF 00                   |
|Reserved |00 BF ED 12                |12 ED BF 00                   |
|Reserved |00 BF EE 11                |11 EE BF 00                   |
|Reserved |00 BF EF 10                |10 EF BF 00                   |
|Reserved |00 BF F0 0F                |0F F0 BF 00                   |
|Reserved |00 BF F1 0E                |0E F1 BF 00                   |
|Reserved |00 BF F2 0D                |0D F2 BF 00                   |
|Reserved |00 BF F3 0C                |0C F3 BF 00                   |
|Reserved |00 BF F4 0B                |0B F4 BF 00                   |
|Reserved |00 BF F5 0A                |0A F5 BF 00                   |
|Reserved |00 BF F6 09                |09 F6 BF 00                   |
|Reserved |00 BF F7 08                |08 F7 BF 00                   |
|Reserved |00 BF F8 07                |07 F8 BF 00                   |
|Reserved |00 BF F9 06                |06 F9 BF 00                   |
|Reserved |00 BF FA 05                |05 FA BF 00                   |
|Reserved |00 BF FB 04                |04 FB BF 00                   |
|Reserved |00 BF FC 03                |03 FC BF 00                   |
|Reserved |00 BF FD 02                |02 FD BF 00                   |
|Reserved |00 BF FE 01                |01 FE BF 00                   |
|Reserved |00 BF FF 00                |00 FF BF 00                   |