using UnityEngine;

[CreateAssetMenu(fileName= "Credits",menuName = "Creditos")]
public class Credits : ScriptableObject
{
   public string[]  Name;
   public string[]  Rol;
   public string _agredecimientos;
   
   public void AddCredit(string name , string rol)
   {
      string[] _newNames = new string[Name.Length + 1];
      string[] _newRol= new string[Rol.Length + 1];

      for (int i = 0; i < Name.Length; i++)
      {
         _newNames[i] = Name[i];
         _newRol[i] = Rol[i];
      }

      _newNames[_newNames.Length - 1] = name;
      _newRol[_newRol.Length - 1] = rol;

      Name = _newNames;
      Rol = _newRol;
   }
   
}

    

