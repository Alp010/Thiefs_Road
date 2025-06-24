using UnityEngine;

public class BrakeButtonListener : MonoBehaviour
{
    private bool isBraking;

    // Dışarıdan sadece okunabilir, içeride değiştirilebilir.
    public bool IsBraking => isBraking;

    // Freni başlat (örn: UI butonuna basılınca)
    public void StartBraking()
    {
        isBraking = true;
        Debug.Log("Frene Basılıyor");
    }

    // Freni durdur (örn: UI butonundan parmak çekilince)
    public void StopBraking()
    {
        isBraking = false;
        Debug.Log("Frene Basılmıyor");
    }
}