namespace Character.Models
{
    public class CharacterDataDummy : ICharacterData
    {
        public string Color => "#00FF00";
        public StyleNameData StyleName => new StyleNameData("", "", 0);
    }
}