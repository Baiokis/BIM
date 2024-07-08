using EzDimension.dims;
using UnityEngine;
using UnityEngine.UI;

public class DimensionUI : MonoBehaviour
{
    public EzDimStarter ezDimStarter; // Arraste e solte o EzDimStarter aqui no inspector
    public Button p2pMeasureButton;
    public Button areaMeasureButton;
    public Button angleMeasureButton;
    public Button linearMeasureButton;
    public Button alignMeasureButton;
    public Button deleteSelectionButton;
    public Button hideSelectionButton;
    public Button unhideSelectionButton;

    void Start()
    {
        // Adiciona os listeners aos botões
        p2pMeasureButton.onClick.AddListener(ezDimStarter.EzPointToPointDimension);
        areaMeasureButton.onClick.AddListener(ezDimStarter.EzAreaMeasure);
        angleMeasureButton.onClick.AddListener(ezDimStarter.EzAngledDimension);
        linearMeasureButton.onClick.AddListener(ezDimStarter.EzLinearDimension);
        alignMeasureButton.onClick.AddListener(ezDimStarter.EzAlignedDimension);
        deleteSelectionButton.onClick.AddListener(ezDimStarter.DeleteSelectedDimensions);
        hideSelectionButton.onClick.AddListener(ezDimStarter.HideSelectedDimensions);
        unhideSelectionButton.onClick.AddListener(ezDimStarter.UnhideAll);
    }
}
