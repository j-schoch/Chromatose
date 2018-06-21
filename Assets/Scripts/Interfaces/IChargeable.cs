using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargeable
{
	void OnChargeBegin();
	void OnChargeComplete(float finalCharge);
	void SetCharge(float charge);
	void ResetCharge();
}
