using System;
using UnityEngine;
using UnityEngine.UI;

public class CreditsDisplay : MonoBehaviour
{
    public Credits _Credits;
    public Text _nameText;
    public Text _rolText;
    public float _speed = 3f;
    private float _destroyY = 1080f;
    private Vector3 _initialPosition;
    public Text _agradecimientosText;
    
    
    void Start()
    {
        _initialPosition = transform.position;
        
        IniatilizeCredits();
        
        UpdateText();
    }

     void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y > _destroyY)
        {
            ResetCredits(); 
        }
    }

     private void IniatilizeCredits()
     {
         
         _Credits.AddCredit("Ivan Herrera","Project Manager, Technical Reviewer");
         _Credits.AddCredit("Fernando, Ricardo","UI");
         _Credits.AddCredit("Ricardo"," Creditos");
         _Credits.AddCredit("Fernando"," Sistema de cámara");
         _Credits.AddCredit("Fernando"," Localización");
         _Credits.AddCredit("Fernando,Abraham,Ricardo,Jair"," Integración de arte");
         _Credits.AddCredit("Abraham"," Sistema de guardado");
         _Credits.AddCredit("Jair,Abraham"," Diseño y Armado del Tutorial");
         _Credits.AddCredit("Abraham"," Diseño y Armado del Nivel 1");
         _Credits.AddCredit("Fernando"," Diseño y Armado del Nivel 2");
         _Credits.AddCredit("Jair"," Diseño y Armado del Nivel 3");
         _Credits.AddCredit("Jair"," Diseño y Armado del Jefe");
         
         _Credits._agredecimientos = "gracias";
     }
     
     void ResetCredits()
     {
         transform.position = _initialPosition;
         UpdateText();
     }

     private void UpdateText()
     {
         _nameText.text = ""; 
         _rolText.text = "";
         _agradecimientosText.text = "";
         
            for (int i = 0; i < _Credits.Name.Length; i++)
             {
                 _nameText.text += "<color=green>" + _Credits.Name[i] + "</color>\n"; 
                 _rolText.text += "<color=blue>" + _Credits.Rol[i] + "</color>\n"; 
             }

             _agradecimientosText.text = _Credits._agredecimientos;
     }
}
    

