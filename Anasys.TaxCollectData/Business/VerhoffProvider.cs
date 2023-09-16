using Anasys.TaxCollectData.Abstraction;

namespace Anasys.TaxCollectData.Business;

internal class VerhoffProvider : IVerhoeffProvider
{
    private readonly int[,] _multiplicationTable = new int[10, 10]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 },
        { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 },
        { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 },
        { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 },
        { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 },
        { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 },
        { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 },
        { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 },
        { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }
    };

    private readonly int[,] _permutationTable = new int[8, 10]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 },
        { 5, 8, 0, 3, 7, 9, 6, 1, 4, 2 },
        { 8, 9, 1, 6, 0, 4, 3, 5, 2, 7 },
        { 9, 4, 5, 3, 1, 2, 6, 8, 7, 0 },
        { 4, 2, 8, 6, 5, 7, 3, 9, 0, 1 },
        { 2, 7, 9, 3, 8, 0, 6, 4, 1, 5 },
        { 7, 0, 4, 6, 9, 1, 3, 2, 5, 8 }
    };

    private readonly int[] _inverseTable = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

    public bool ValidateVerhoeff(string num)
    {
        var index1 = 0;
        var reversedIntArray = StringToReversedIntArray(num);
        for (var index2 = 0; index2 < reversedIntArray.Length; ++index2)
            index1 = _multiplicationTable[index1, _permutationTable[index2 % 8, reversedIntArray[index2]]];
        return index1 == 0;
    }

    public string GenerateVerhoeff(string num)
    {
        var index1 = 0;
        var reversedIntArray = StringToReversedIntArray(num);
        for (var index2 = 0; index2 < reversedIntArray.Length; ++index2)
            index1 = _multiplicationTable[index1, _permutationTable[(index2 + 1) % 8, reversedIntArray[index2]]];
        return _inverseTable[index1].ToString();
    }

    private static int[] StringToReversedIntArray(string num)
    {
        var reversedIntArray = new int[num.Length];
        for (var startIndex = 0; startIndex < num.Length; ++startIndex)
            reversedIntArray[startIndex] = int.Parse(num.Substring(startIndex, 1));
        Array.Reverse((Array)reversedIntArray);
        return reversedIntArray;
    }
}