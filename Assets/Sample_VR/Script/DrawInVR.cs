using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

using EzDimension;
using System.Collections.Generic;
using EzDimension.dims;


public class DrawInVR : MonoBehaviour
{
    [HideInInspector]
    public RaycastHit hoveredHit;

    int primaryButtonCounter = 0;
    int secondaryButtonCounter = 0;

    bool firstStep;
    bool secoundStep;
    bool thirdStep;
    bool fourthStep;
    bool fifthStep;
    bool sixthStep;
    bool primaryIsPressedThisFrame;

    bool tempState;
    bool selectButton;
    bool secondaryButtonState = false;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    public PrimaryButtonEvent primaryButtonPress;
    private bool lastButtonState = false;
    private List<InputDevice> devicesWithPrimaryButton;

    private void Awake()
    {
        if (primaryButtonPress == null)
        {
            primaryButtonPress = new PrimaryButtonEvent();
        }

        devicesWithPrimaryButton = new List<InputDevice>();
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        devicesWithPrimaryButton.Clear();
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out discardedValue))
        {
            devicesWithPrimaryButton.Add(device); // Add any devices that have a primary button.
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (devicesWithPrimaryButton.Contains(device))
            devicesWithPrimaryButton.Remove(device);
    }

    void Update()
    {
        tempState = false;
        foreach (var device in devicesWithPrimaryButton)
        {
            bool primaryButtonState = false;

            tempState = device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) // did get a value
                        && primaryButtonState // the value we got
                        || tempState; // cumulative result from other controllers
            // selectButton
            device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonState);
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            if (primaryButtonCounter == 0)
            {
                // do something when primary button is pressed at this frame

                // bool mode
                primaryIsPressedThisFrame = true;
                // step mode
                if (firstStep && !secoundStep && !thirdStep && !fourthStep && !fifthStep && !sixthStep)
                    secoundStep = true;
                else if (firstStep && secoundStep && !thirdStep && !fourthStep && !fifthStep && !sixthStep)
                    thirdStep = true;
                else if (firstStep && secoundStep && thirdStep && !fourthStep && !fifthStep && !sixthStep)
                    fourthStep = true;
                else if (!firstStep && secoundStep && thirdStep && fourthStep && !fifthStep && !sixthStep)
                    fifthStep = true;
                else if (!firstStep && secoundStep && thirdStep && fourthStep && fifthStep && !sixthStep)
                    sixthStep = true;
                // end

                primaryButtonCounter = 1;
            }
        }
        else
            primaryButtonCounter = 0;

        if (secondaryButtonState) // true while is pressed
        {
            selectButton = false;
            if (secondaryButtonCounter == 0)
            {
                //do something when secondary button is pressed at this frame
                selectButton = true;
                //end
                secondaryButtonCounter = 1;
            }
        }
        else
            secondaryButtonCounter = 0;
    }
    public void VR_EzPointToPointDimension(EzDimStarter _starterScript)
    {
        if (!_starterScript.isCreating)
        {
            firstStep = true;
            secoundStep = thirdStep = fourthStep = fifthStep = sixthStep = false;
            _starterScript.isCreating = true;  // this bool will set to false after draw the dimension.
            _starterScript.SelectionList.Clear();
            Funcs.UpdateAll(_starterScript, _starterScript.DimensionsList, _starterScript.SelectionList);
            GameObject EzPointToPointDimensionGO = new GameObject("VR_EzPointToPointDimension");
            EzPointToPointDimensionGO.transform.parent = _starterScript.transform;
            var p2PDim = EzPointToPointDimensionGO.AddComponent<PointToPointDimension>();

            if (_starterScript.createDimensionCort != null)
                StopCoroutine(_starterScript.createDimensionCort);

            _starterScript.createDimensionCort = StartCoroutine(CreatePointToPointDimension(_starterScript, p2PDim));

            _starterScript.DimensionsList.Add(EzPointToPointDimensionGO.gameObject); // add the parent GO to the list.
        }
    }
    public void VR_EzLinearDimension(EzDimStarter _starterScript)
    {
        firstStep = true;
        secoundStep = thirdStep = fourthStep = fifthStep = sixthStep = false;
        _starterScript.isCreating = true;  // this bool will set to false after draw the dimension.
        _starterScript.SelectionList.Clear();
        Funcs.UpdateAll(_starterScript, _starterScript.DimensionsList, _starterScript.SelectionList);
        GameObject EzLineardDimensionGO = new GameObject("VR_EzLinearDimension");
        EzLineardDimensionGO.transform.parent = _starterScript.transform;
        var linDim = EzLineardDimensionGO.AddComponent<LinearDimension>();

        if (_starterScript.createDimensionCort != null)
            StopCoroutine(_starterScript.createDimensionCort);

        _starterScript.createDimensionCort = StartCoroutine(CreateLinearDimension(_starterScript, linDim));

        _starterScript.DimensionsList.Add(EzLineardDimensionGO.gameObject); // add the parent GO to the list.
    }
    public void VR_EzAlignedDimension(EzDimStarter _starterScript)
    {
        firstStep = true;
        secoundStep = thirdStep = fourthStep = fifthStep = sixthStep = false;
        _starterScript.isCreating = true;  // this bool will set to false after draw the dimension.
        _starterScript.SelectionList.Clear();
        Funcs.UpdateAll(_starterScript, _starterScript.DimensionsList, _starterScript.SelectionList);
        GameObject EzAlignedDimensionGO = new GameObject("VR_EzAlignedDimension");
        EzAlignedDimensionGO.transform.parent = _starterScript.transform;
        var alignDim = EzAlignedDimensionGO.AddComponent<AlignedDimension>();

        if (_starterScript.createDimensionCort != null)
            StopCoroutine(_starterScript.createDimensionCort);

        _starterScript.createDimensionCort = StartCoroutine(CreateAlignedDimension(_starterScript, alignDim));

        _starterScript.DimensionsList.Add(EzAlignedDimensionGO.gameObject); // add the parent GO to the list.
    }
    public void VR_EzAngleDimension(EzDimStarter _starterScript)
    {
        firstStep = true;
        secoundStep = thirdStep = fourthStep = fifthStep = sixthStep = false;
        _starterScript.isCreating = true;  // this bool will set to false after draw the dimension.
        _starterScript.SelectionList.Clear();
        Funcs.UpdateAll(_starterScript, _starterScript.DimensionsList, _starterScript.SelectionList);
        GameObject EzAngleDimensionGO = new GameObject("VR_EzAngleDimension");
        EzAngleDimensionGO.transform.parent = _starterScript.transform;
        var angleDim = EzAngleDimensionGO.AddComponent<AngleDimension>();

        if (_starterScript.createDimensionCort != null)
            StopCoroutine(_starterScript.createDimensionCort);

        _starterScript.createDimensionCort = StartCoroutine(CreateAngleDimension(_starterScript, angleDim));

        _starterScript.DimensionsList.Add(EzAngleDimensionGO.gameObject); // add the parent GO to the list.
    }
    public void VR_EzAreaMeasure(EzDimStarter _starterScript)
    {
        if (!_starterScript.isCreating)
        {
            //firstStep = true;
            //secoundStep = thirdStep = fourthStep = fifthStep = sixthStep = false;
             primaryIsPressedThisFrame = false;
            _starterScript.isCreating = true;  // this bool will set to false after draw the dimension.
            _starterScript.SelectionList.Clear();
            Funcs.UpdateAll(_starterScript, _starterScript.DimensionsList, _starterScript.SelectionList);
            GameObject EzAreaMeasureGO = new GameObject("VR_EzAreaMeasure");
            EzAreaMeasureGO.transform.parent = _starterScript.transform;
            var Area = EzAreaMeasureGO.AddComponent<LinearAreaMeasure>();

            if (_starterScript.createDimensionCort != null)
                StopCoroutine(_starterScript.createDimensionCort);

            _starterScript.createDimensionCort = StartCoroutine(CreateAreaMeasure(_starterScript, Area));

            _starterScript.DimensionsList.Add(EzAreaMeasureGO.gameObject); // add the parent GO to the list.
        }
    }
    public void VR_HighlightHoveredDimension(EzDimStarter _starterScript)
    {
        GameObject hoveredGO;

        bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hoveredHit);

        if (ray)
        {
            _starterScript.hit = hoveredHit;
            hoveredGO = Funcs.IsHoveredOnDimension(_starterScript, _starterScript.SelectionList, hoveredHit);
        }
        else
            hoveredGO = null;

        if (hoveredGO != null && _starterScript.oldHoveredGo != hoveredGO)
            _starterScript.onHovered.Invoke(hoveredGO);

        _starterScript.oldHoveredGo = hoveredGO;
    }
    public void VR_SelectDimension(EzDimStarter _starterScript)
    {
        RaycastHit selectedHit;
        bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out selectedHit);
        if (ray && selectButton)
        {
            //_starterScript.hit = selectedHit;

            _starterScript.isOldSelectionListEmpty = _starterScript.SelectionList.Count == 0;

            Funcs.SelectDimension(_starterScript, _starterScript.DimensionsList, _starterScript.SelectionList, selectedHit);

            if (_starterScript.SelectionList.Count != 0 || !_starterScript.isOldSelectionListEmpty)
            {
                _starterScript.onSelectionChanged.Invoke(_starterScript.SelectionList);
            }
        }
    }

    IEnumerator CreatePointToPointDimension(EzDimStarter _starterScript, PointToPointDimension _p2PDim)
    {
        _p2PDim.secondDrawStep = false;

        while (_p2PDim.isDone != true)
        {
            if (firstStep && secoundStep && !thirdStep && !_p2PDim.secondDrawStep && _starterScript.isCreating)
            {
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _p2PDim.pointA = hit.point;
                    _p2PDim.objectA = hit.transform.gameObject;
                    _p2PDim.objectATransformGO.transform.position = hit.transform.position;
                    _p2PDim.objectATransformGO.transform.rotation = hit.transform.rotation;
                    _p2PDim.objectATransformGO.transform.localScale = hit.transform.localScale;
                    _p2PDim.pointATransformGO.transform.position = _p2PDim.pointA;

                    if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                    {
                        hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().p2PDimComponentsList.Add(_p2PDim);
                        hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                    }
                    else
                    {
                        var p2PDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                        p2PDimComp.p2PDimComponentsList.Add(_p2PDim);
                    }

                    _p2PDim.secondDrawStep = true;
                }

            }
            else if (_p2PDim.secondDrawStep == true)
            {
                Funcs.SetActiveAllChilds(_p2PDim.gameObject.transform, true);

                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _p2PDim.pointB = hit.point;
                    _p2PDim.objectB = hit.transform.gameObject;
                    _p2PDim.objectBTransformGO.transform.position = hit.transform.position;
                    _p2PDim.objectBTransformGO.transform.rotation = hit.transform.rotation;
                    _p2PDim.objectBTransformGO.transform.localScale = hit.transform.localScale;
                    _p2PDim.pointBTransformGO.transform.position = _p2PDim.pointB;

                    _p2PDim.UpdateDimension(_p2PDim.pointA, _p2PDim.pointB, _p2PDim.lineThickness, _p2PDim.textSize, _p2PDim.textOffset,
                        _p2PDim.numberColor, _p2PDim.mainColor, _p2PDim.arrowColor,
                        _p2PDim.mainLineMat, _p2PDim.arrowMat, _p2PDim.cameraTransform, _p2PDim.mainParent, _p2PDim.arrowHeight,
                        internalFuncs.LengthUnitCalculator(_starterScript), _p2PDim.textTowardsCameraOffset, _p2PDim.NormalOffset);

                    if (firstStep && secoundStep && thirdStep)
                    {
                        if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                        {
                            hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().p2PDimComponentsList.Add(_p2PDim);
                            hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                        }
                        else
                        {
                            var p2PDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                            p2PDimComp.p2PDimComponentsList.Add(_p2PDim);
                        }
                        _starterScript.isCreating = false; // end of drawing of the dimension.
                        _p2PDim.secondDrawStep = false;
                        _p2PDim.isDone = true;
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator CreateLinearDimension(EzDimStarter _starterScript, LinearDimension _linDim)
    {
        _linDim.secondDrawStep = false;

        while (_linDim.isDone != true)
        {
            if (firstStep && secoundStep && !thirdStep && !_linDim.secondDrawStep && _starterScript.isCreating)
            {
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _linDim.pointA = hit.point;
                    _linDim.objectA = hit.transform.gameObject;
                    _linDim.objectATransformGO.transform.position = hit.transform.position;
                    _linDim.objectATransformGO.transform.rotation = hit.transform.rotation;
                    _linDim.objectATransformGO.transform.localScale = hit.transform.localScale;
                    _linDim.pointATransformGO.transform.position = _linDim.pointA;

                    if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                    {
                        hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().linDimComponentsList.Add(_linDim);
                        hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                    }
                    else
                    {
                        var LinDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                        LinDimComp.linDimComponentsList.Add(_linDim);
                    }
                    _linDim.secondDrawStep = true;
                }
            }
            else if (_linDim.secondDrawStep == true)
            {
                Funcs.SetActiveAllChilds(_linDim.gameObject.transform, true);
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _linDim.pointB = hit.point;
                    _linDim.objectB = hit.transform.gameObject;
                    _linDim.objectBTransformGO.transform.position = hit.transform.position;
                    _linDim.objectBTransformGO.transform.rotation = hit.transform.rotation;
                    _linDim.objectBTransformGO.transform.localScale = hit.transform.localScale;
                    _linDim.pointBTransformGO.transform.position = _linDim.pointB;

                    _linDim.UpdateDimension(_linDim.pointA, _linDim.pointB, _linDim.mainLineThickness, _linDim.secondaryLinesThickness,
                        _linDim.secondaryLinesExtend, _linDim.textSize, _linDim.textOffset, _linDim.mainLineMat, _linDim.secondaryLinesMat,
                        _linDim.arrowMat, _linDim.numberColor, _linDim.mainColor, _linDim.secondaryColor, _linDim.arrowColor,
                        _linDim.cameraTransform, _linDim.mainParent, _linDim.arrowHeight, _linDim.measurementDirection,
                        _linDim.offsetDirection, _linDim.offsetDistance, _linDim.autoTextPosition, _linDim.flipTextPosition,
                        internalFuncs.LengthUnitCalculator(_starterScript), _linDim.textTowardsCameraOffset, _linDim.NormalOffset);

                    if (firstStep && secoundStep && thirdStep)
                    {
                        if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                        {
                            hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().linDimComponentsList.Add(_linDim);
                            hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                        }
                        else
                        {
                            var linDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                            linDimComp.linDimComponentsList.Add(_linDim);
                        }
                        _starterScript.isCreating = false; // end of drawing of the dimension.
                        _linDim.isDone = true;
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator CreateAlignedDimension(EzDimStarter _starterScript, AlignedDimension _alignDim)
    {
        _alignDim.secondDrawStep = false;

        while (_alignDim.isDone != true)
        {
            if (firstStep && secoundStep && !thirdStep && !_alignDim.secondDrawStep && _starterScript.isCreating)
            {
                print("1");
                _starterScript.stepA = false;
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _alignDim.pointA = hit.point;
                    _alignDim.objectA = hit.transform.gameObject;
                    _alignDim.objectATransformGO.transform.position = hit.transform.position;
                    _alignDim.objectATransformGO.transform.rotation = hit.transform.rotation;
                    _alignDim.objectATransformGO.transform.localScale = hit.transform.localScale;
                    _alignDim.pointATransformGO.transform.position = _alignDim.pointA;

                    if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                    {
                        print("2");
                        hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().alignDimComponentsList.Add(_alignDim);
                        hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                    }
                    else
                    {
                        print("3");
                        var alignDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                        alignDimComp.alignDimComponentsList.Add(_alignDim);
                    }
                    _alignDim.secondDrawStep = true;
                }
            }
            else if (_alignDim.secondDrawStep == true)
            {
                print("4");
                Funcs.SetActiveAllChilds(_alignDim.gameObject.transform, true);
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray & _alignDim.measurementPlane != Funcs.MeasurementPlane.Free)
                {
                    print("5");
                    _alignDim.pointB = hit.point;
                    _alignDim.objectB = hit.transform.gameObject;
                    _alignDim.objectBTransformGO.transform.position = hit.transform.position;
                    _alignDim.objectBTransformGO.transform.rotation = hit.transform.rotation;
                    _alignDim.objectBTransformGO.transform.localScale = hit.transform.localScale;
                    _alignDim.pointBTransformGO.transform.position = _alignDim.pointB;

                    _alignDim.UpdateDimension(_alignDim.pointA, _alignDim.pointB, _alignDim.mainLineThickness,
                        _alignDim.secondaryLinesThickness, _alignDim.secondaryLinesExtend, _alignDim.textSize, _alignDim.textOffset,
                        _alignDim.numberColor, _alignDim.mainColor, _alignDim.secondaryColor, _alignDim.arrowColor,
                       _alignDim.mainLineMat, _alignDim.secondaryLinesMat, _alignDim.arrowMat, _alignDim.cameraTransform,
                       _alignDim.mainParent, _alignDim.arrowHeight, _alignDim.measurementPlane, _alignDim.offsetDistance,
                       internalFuncs.LengthUnitCalculator(_starterScript), _alignDim.autoTextPosition, _alignDim.flipTextPosition,
                       _alignDim.textTowardsCameraOffset, _alignDim.textOffset);

                    if (firstStep && secoundStep && thirdStep)
                    {
                        print("6");
                        if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                        {
                            hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().alignDimComponentsList.Add(_alignDim);
                            hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                        }
                        else
                        {
                            var alignDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                            alignDimComp.alignDimComponentsList.Add(_alignDim);
                        }
                        _starterScript.isCreating = false; // end of drawing of the dimension.
                        _alignDim.isDone = true;
                    }
                }
                else if (ray & _alignDim.measurementPlane == Funcs.MeasurementPlane.Free)
                {
                    if (!_starterScript.stepA)
                    {
                        _alignDim.pointB = hit.point;
                        _alignDim.objectB = hit.transform.gameObject;
                        _alignDim.objectBTransformGO.transform.position = hit.transform.position;
                        _alignDim.objectBTransformGO.transform.rotation = hit.transform.rotation;
                        _alignDim.objectBTransformGO.transform.localScale = hit.transform.localScale;
                        _alignDim.pointBTransformGO.transform.position = _alignDim.pointB;
                    }

                    _alignDim.UpdateDimension(_alignDim.pointA, _alignDim.pointB, _alignDim.mainLineThickness,
                          _alignDim.secondaryLinesThickness, _alignDim.secondaryLinesExtend, _alignDim.textSize, _alignDim.textOffset,
                          _alignDim.numberColor, _alignDim.mainColor, _alignDim.secondaryColor, _alignDim.arrowColor,
                         _alignDim.mainLineMat, _alignDim.secondaryLinesMat, _alignDim.arrowMat, _alignDim.cameraTransform,
                         _alignDim.mainParent, _alignDim.arrowHeight, _alignDim.measurementPlane, _alignDim.offsetDistance,
                         internalFuncs.LengthUnitCalculator(_starterScript), _alignDim.autoTextPosition, _alignDim.flipTextPosition,
                         _alignDim.textTowardsCameraOffset, _alignDim.textOffset);

                    if (firstStep && secoundStep && thirdStep & !_starterScript.stepA)
                    {
                        print("6");
                        if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                        {
                            hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().alignDimComponentsList.Add(_alignDim);
                            hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                        }
                        else
                        {
                            var alignDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                            alignDimComp.alignDimComponentsList.Add(_alignDim);
                        }
                    }

                    if (_starterScript.stepA)
                    {
                        print("7");

                        Vector3 lineCenter = Vector3.Lerp(_alignDim.pointA, _alignDim.pointB, 0.5f);
                        Vector3 LineDirection = (_alignDim.pointB - _alignDim.pointA).normalized;
                        Plane plane = new Plane(LineDirection, lineCenter);
                        Vector3 hitPointOnPlane = plane.ClosestPointOnPlane(hit.point);


                        if (_starterScript.alignedDimFreeModePlanePrefab != null)
                        {
                            print("8");

                            if (_starterScript.planePrefab == null)
                                _starterScript.planePrefab = GameObject.Instantiate(_starterScript.alignedDimFreeModePlanePrefab, _alignDim.transform);
                        }

                        if (_starterScript.alignedDimFreeModeDirPointer != null)
                        {
                            print("9");
                            if (_starterScript.directionPointer == null)
                                _starterScript.directionPointer = GameObject.Instantiate(_starterScript.alignedDimFreeModeDirPointer, _alignDim.transform);
                        }

                        if (_starterScript.directionPointer != null)
                        {
                            print("10");
                            _alignDim.DirectionPointInFreeMode = hitPointOnPlane;
                            _starterScript.directionPointer.transform.position = hitPointOnPlane;
                        }

                        if (_starterScript.planePrefab != null)
                        {
                            print("11");

                            _starterScript.planePrefab.transform.position = lineCenter;
                            _starterScript.planePrefab.transform.rotation = Quaternion.LookRotation(LineDirection);

                            if (_starterScript.planePrefabScaleMultiplier > 0)
                            {
                                float scaleNumber = (hitPointOnPlane - lineCenter).magnitude;
                                _starterScript.planePrefab.transform.localScale = new Vector3(scaleNumber, scaleNumber, scaleNumber) * _starterScript.planePrefabScaleMultiplier;
                            }
                            else
                            {
                                _starterScript.planePrefab.transform.localScale = Vector3.one;
                            }
                        }

                        _alignDim.UpdateDimension(_alignDim.pointA, _alignDim.pointB, _alignDim.mainLineThickness,
                            _alignDim.secondaryLinesThickness, _alignDim.secondaryLinesExtend, _alignDim.textSize, _alignDim.textOffset,
                            _alignDim.numberColor, _alignDim.mainColor, _alignDim.secondaryColor, _alignDim.arrowColor,
                            _alignDim.mainLineMat, _alignDim.secondaryLinesMat, _alignDim.arrowMat, _alignDim.cameraTransform,
                            _alignDim.mainParent, _alignDim.arrowHeight, _alignDim.measurementPlane, _alignDim.offsetDistance,
                            internalFuncs.LengthUnitCalculator(_starterScript), _alignDim.autoTextPosition, _alignDim.flipTextPosition,
                            _alignDim.textTowardsCameraOffset,_alignDim.textOffset);

                        if (firstStep && secoundStep && thirdStep && fourthStep)
                        {
                            print("12");
                            if (_starterScript.directionPointer != null)
                                GameObject.Destroy(_starterScript.directionPointer);

                            if (_starterScript.planePrefab != null)
                                GameObject.Destroy(_starterScript.planePrefab);

                            _starterScript.stepA = false;
                            _alignDim.secondDrawStep = false;
                            _alignDim.isDone = true;
                            _starterScript.isCreating = false; // end of drawing of the dimension.
                        }
                    }

                    if (firstStep && secoundStep && thirdStep)
                        _starterScript.stepA = true;
                }
            }
            yield return null;
        }
    }
    IEnumerator CreateAngleDimension(EzDimStarter _starterScript, AngleDimension _angleDim)
    {
        while (_angleDim.isDone != true)
        {
            if (firstStep && secoundStep && !thirdStep && !_angleDim.trueOnFirstStep && !_angleDim.trueOnSecoundStep && !fourthStep && _starterScript.isCreating)
            {
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _angleDim.firstPointHitNormal = hit.normal;
                    _angleDim.lineAGO.SetActive(true);
                    _angleDim.pointA = hit.point;
                    _angleDim.objectA = hit.transform.gameObject;
                    _angleDim.objectATransformGO.transform.position = hit.transform.position;
                    _angleDim.objectATransformGO.transform.rotation = hit.transform.rotation;
                    _angleDim.objectATransformGO.transform.localScale = hit.transform.localScale;
                    _angleDim.pointATransformGO.transform.position = hit.point;

                    if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                    {
                        hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().angleDimComponentsList.Add(_angleDim);
                        hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                    }
                    else
                    {
                        var angleDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                        angleDimComp.angleDimComponentsList.Add(_angleDim);
                    }

                    _angleDim.trueOnFirstStep = true;
                }
            }
            else if (_angleDim.trueOnFirstStep && !_angleDim.trueOnSecoundStep)
            {

                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _angleDim.pointB = hit.point;

                    if (_angleDim.angleMeasurementPlane == Funcs.AngleMeasurmentPlane.XZ)
                        _angleDim.pointBProxy = new Vector3(_angleDim.pointB.x, _angleDim.pointA.y, _angleDim.pointB.z);
                    else if (_angleDim.angleMeasurementPlane == Funcs.AngleMeasurmentPlane.XY)
                        _angleDim.pointBProxy = new Vector3(_angleDim.pointB.x, _angleDim.pointB.y, _angleDim.pointA.z);
                    else if (_angleDim.angleMeasurementPlane == Funcs.AngleMeasurmentPlane.ZY)
                        _angleDim.pointBProxy = new Vector3(_angleDim.pointA.x, _angleDim.pointB.y, _angleDim.pointB.z);
                    else _angleDim.pointBProxy = _angleDim.pointB;

                    _angleDim.UpdateDimension(_angleDim.pointA, _angleDim.pointBProxy, _angleDim.pointCProxy, _angleDim.linesThickness, _angleDim.textSize,
                        _angleDim.arcScale, _angleDim.normalOffset,
                        _angleDim.textOffsetFromCenter, _angleDim.textOffsetIfNotFit, _angleDim.numberColor, _angleDim.mainColor, _angleDim.arcColor,
                        _angleDim.cameraTransform, _angleDim.mainParent, _angleDim.arcMat, _angleDim.mainLinesMat, _angleDim.angleMeasurementPlane,
                        internalFuncs.LengthUnitCalculator(_starterScript));

                    if (firstStep && secoundStep && thirdStep && !fourthStep)
                    {
                        _angleDim.numberGO.GetComponent<BoxCollider>().enabled = false; // disable box collider to avoid detect any point of number as target point

                        Funcs.SetActiveAllChilds(_angleDim.gameObject.transform, true);
                        _angleDim.arcParentGO.transform.position = _angleDim.pointBProxy;
                        _angleDim.objectB = hit.transform.gameObject;
                        _angleDim.objectBTransformGO.transform.position = hit.transform.position;
                        _angleDim.objectBTransformGO.transform.rotation = hit.transform.rotation;
                        _angleDim.objectBTransformGO.transform.localScale = hit.transform.localScale;
                        _angleDim.pointBTransformGO.transform.position = _angleDim.pointB;

                        if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                        {
                            hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().angleDimComponentsList.Add(_angleDim);
                            hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                        }
                        else
                        {
                            var angleDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                            angleDimComp.angleDimComponentsList.Add(_angleDim);
                        }

                        _angleDim.trueOnSecoundStep = true;
                    }

                }
            }
            else if (_angleDim.trueOnFirstStep && _angleDim.trueOnSecoundStep)
            {
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    _angleDim.pointC = hit.point;
                    if (_angleDim.angleMeasurementPlane == Funcs.AngleMeasurmentPlane.XZ)
                        _angleDim.pointCProxy = new Vector3(_angleDim.pointC.x, _angleDim.pointA.y, _angleDim.pointC.z);
                    else if (_angleDim.angleMeasurementPlane == Funcs.AngleMeasurmentPlane.XY)
                        _angleDim.pointCProxy = new Vector3(_angleDim.pointC.x, _angleDim.pointC.y, _angleDim.pointA.z);
                    else if (_angleDim.angleMeasurementPlane == Funcs.AngleMeasurmentPlane.ZY)
                        _angleDim.pointCProxy = new Vector3(_angleDim.pointA.x, _angleDim.pointC.y, _angleDim.pointC.z);
                    else
                        _angleDim.pointCProxy = _angleDim.pointC;

                    _angleDim.UpdateDimension(_angleDim.pointA, _angleDim.pointBProxy, _angleDim.pointCProxy, _angleDim.linesThickness, _angleDim.textSize,
                        _angleDim.arcScale, _angleDim.normalOffset,
                        _angleDim.textOffsetFromCenter, _angleDim.textOffsetIfNotFit, _angleDim.numberColor, _angleDim.mainColor, _angleDim.arcColor,
                        _angleDim.cameraTransform, _angleDim.mainParent, _angleDim.arcMat, _angleDim.mainLinesMat, _angleDim.angleMeasurementPlane,
                        internalFuncs.LengthUnitCalculator(_starterScript));

                    if (firstStep && secoundStep && thirdStep && fourthStep)
                    {
                        _angleDim.plane = new Plane(_angleDim.pointA, _angleDim.pointBProxy, _angleDim.pointCProxy); // cut the space by a plane from main points(A,B,C). plane is only required for Free angle dimension.

                        _angleDim.objectC = hit.transform.gameObject;
                        _angleDim.objectCTransformGO.transform.position = hit.transform.position;
                        _angleDim.objectCTransformGO.transform.rotation = hit.transform.rotation;
                        _angleDim.objectCTransformGO.transform.localScale = hit.transform.localScale;
                        _angleDim.pointCTransformGO.transform.position = _angleDim.pointC;

                        if (!hit.transform.gameObject.TryGetComponent<EzDynamicTargetObject>(out EzDynamicTargetObject EzDynamicTarget))
                        {
                            hit.transform.gameObject.AddComponent<EzDynamicTargetObject>().angleDimComponentsList.Add(_angleDim);
                            hit.transform.gameObject.GetComponent<EzDynamicTargetObject>().starterScript = _starterScript;
                        }
                        else
                        {
                            var angleDimComp = hit.transform.gameObject.GetComponent<EzDynamicTargetObject>();
                            angleDimComp.angleDimComponentsList.Add(_angleDim);
                        }

                        _angleDim.trueOnFirstStep = false;
                        _angleDim.trueOnSecoundStep = false;
                        _angleDim.numberGO.GetComponent<BoxCollider>().enabled = true; // it was disable to avoid detection as target while creating.
                        _starterScript.isCreating = false; // end of drawing of the dimension.
                        _angleDim.isDone = true;
                    }

                }

            }
            yield return null;
        }

    }
    IEnumerator CreateAreaMeasure(EzDimStarter _starterScript, LinearAreaMeasure _AreaMeasurment)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 hitNormal = Vector3.up;

        while (!_AreaMeasurment.isDone)
        {
            if (_AreaMeasurment.drawMode)
            {
                RaycastHit hit;
                bool ray = rayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (ray)
                {
                    GameObject handle;

                    if (primaryIsPressedThisFrame && _starterScript.isCreating && hit.transform != null)
                    {
                        // if (_starterScript.areaHandlesPrefab != null)
                        // {
                        //     GameObject handle = Instantiate(_starterScript.areaHandlesPrefab);
                        //     handle.transform.localScale *= _starterScript.areaHandlesScale;
                        //     handle.gameObject.name = "Handle";
                        //     handle.transform.position = hit.point;
                        //     handle.transform.parent = _AreaMeasurment.transform;
                        //     var dynamicTarget = handle.AddComponent<EzDynamicTargetObject>();
                        //     dynamicTarget.areaComponentsList.Add(_AreaMeasurment);
                        //     dynamicTarget.areaMeasureRoot = _AreaMeasurment;
                        //     dynamicTarget.starterScript = _starterScript;
                        //     _AreaMeasurment.handlesList.Add(handle);
                        //
                        //     hitNormal = _AreaMeasurment.hitNormal = hit.normal;
                        //     rotation = Quaternion.LookRotation(hitNormal, Vector3.up);
                        //
                        //     handle.transform.rotation = rotation;
                        // }
                        // else
                        //     _AreaMeasurment.points.Add(new Vector2(hit.point.x, hit.point.z));
                        //

                        if (_starterScript.areaHandlesPrefab == null)
                            handle = new GameObject("Handle");
                        else
                        {
                            handle = Instantiate(_starterScript.areaHandlesPrefab);
                            handle.transform.localScale *= _starterScript.areaHandlesScale;
                        }

                        handle.gameObject.name = "Handle";
                        handle.transform.parent = _AreaMeasurment.transform;
                        handle.transform.position = hit.point;
                        var dynamicTarget = handle.AddComponent<EzDynamicTargetObject>();
                        dynamicTarget.areaComponentsList.Add(_AreaMeasurment);
                        dynamicTarget.areaMeasureRoot = _AreaMeasurment;
                        dynamicTarget.starterScript = _starterScript;
                        _AreaMeasurment.handlesList.Add(handle);


                        if (_AreaMeasurment.points.Count == 1 || _AreaMeasurment.handlesList.Count == 1)
                        {
                            hitNormal = _AreaMeasurment.hitNormal = hit.normal;
                            rotation = Quaternion.LookRotation(hitNormal, Vector3.up);
                        }

                        handle.transform.rotation = rotation;

                        primaryIsPressedThisFrame = false;
                    }
                }

                if (selectButton && _starterScript.isCreating)
                {
                    if (_starterScript.enableAreaBorderLine)
                        _AreaMeasurment.enableBorderLine = true;

                    _AreaMeasurment.handlesParent.transform.rotation = rotation;

                    _AreaMeasurment.DrawArea(_AreaMeasurment.points, _starterScript, _starterScript.areaMeasurementPlane, _starterScript.areaLocalYOffset,
                        _starterScript.areaBorderLocalYOffset, _starterScript.areaTextLocalYOffset, _starterScript.areaSurfaceMaterial,
                        _starterScript.areaBorderMaterial, _starterScript.enableAreaBorderLine, _starterScript.areaBorderLineThickness,
                        _starterScript.textSize, _starterScript.cameraTransform, _starterScript.areaNumberPositionOffset,
                        _starterScript.numberColor, _starterScript.areaBorderColor, _starterScript.areaSurfaceColor);

                    _AreaMeasurment.allowDrawSurface = true;
                    _AreaMeasurment.drawMode = false;
                    print("Done");
                    _starterScript.isCreating = false;
                    _AreaMeasurment.isDone = true;
                }
            }
            yield return null;
        }

    }
}


