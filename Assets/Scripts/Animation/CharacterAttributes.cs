using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterAttributes 
{
   public CharacterPartAnimator characterPart;
   public PartVariantColor partVariantColor;
   public PartVariantType partVariantType;

   public CharacterAttributes(CharacterPartAnimator characterPart, PartVariantColor partVariantColor, PartVariantType partVariantType)
   {
       this.characterPart = characterPart;
       this.partVariantColor = partVariantColor;
       this.partVariantType = partVariantType;
   }
}
