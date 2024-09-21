namespace LL.Extensions.Models;

public class Meaning
{
    public string partOfSpeech { get; set; }
    public List<Definition> definitions { get; set; }
    public List<string> synonyms { get; set; }
    public List<string> antonyms { get; set; }
}