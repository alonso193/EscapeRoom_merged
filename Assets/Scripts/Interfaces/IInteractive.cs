using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractive  {
    /**
     * Return the name of the interactive object
     */
    string GetInteractiveName();

    /**
     * ====================
     * H I T  A C T I O N S
     * ====================
     */
    /**
     * Return dictionary with possible hit actions (buttonName, actionName mapping) 
     * to be executed on the interactive object, involving the interactor and 
     * some other object.
     */
    Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other);

    /**
     * Executes the hit action given by the actionName, involving the interactor and 
     * some other object.
     */
    void ExecuteHitAction(string actionName, GameObject interactor, GameObject other);

    /**
     * ========================
     * C A R R Y  A C T I O N S
     * ========================
     */
    /**
     * Return dictionary with possible carry actions (buttonName, actionName mapping) 
     * to be executed on the interactive object, involving only the interactor.
     */
    Dictionary<string, string> GetCarryActions(GameObject interactor);

    /**
     * Executes the carry action given by the actionName, involving only the interactor.
     */
    void ExecuteCarryAction(string actionName, GameObject interactor);

}
