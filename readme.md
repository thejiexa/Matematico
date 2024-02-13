## Matematico

Windows Forms implementation of the Matematico game featuring an AI player that plays with optimal strategy

"Matematico" is an Italian mathematical game played on a 5x5 grid. There is a deck of 52 cards, each containing numbers from 1 to 13, with each number appearing four times. The goal is to create combinations using the numbers on the board to earn the highest number of points. Each combination is associated with a specific number of points

<table align="center">
    <tr>
        <th>Combinations</th>
        <th>Row or column</th>
        <th>Principal diagonal</th>
    </tr>
    <tr>
        <td>For 2 identical numbers</td>
        <td>10</td>
        <td>20</td>
    </tr>
    <tr>
        <td>For 2 pairs of identical numbers</td>
        <td>20</td>
        <td>30</td>
    </tr>
    <tr>
        <td>For 3 identical numbers</td>
        <td>40</td>
        <td>50</td>
    </tr>
    <tr>
        <td>For 5 consecutive numbers, not necessarily in order</td>
        <td>50</td>
        <td>60</td>
    </tr>
    <tr>
        <td>For 3 identical numbers and 2 other identical numbers</td>
        <td>80</td>
        <td>90</td>
    </tr>
    <tr>
        <td>Three times of 1 and two times of 13</td>
        <td>100</td>
        <td>110</td>
    </tr>
    <tr>
        <td>Numbers 1, 13, 12, 11 and 10, but not necessarily in order</td>
        <td>150</td>
        <td>160</td>
    </tr>
    <tr>
        <td>For 4 identical numbers</td>
        <td>160</td>
        <td>170</td>
    </tr>
    <tr>
        <td>For 4 units</td>
        <td>200</td>
        <td>210</td>
    </tr>
</table>