using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController:MonoBehaviour
{
    public Animator anim;
	private bool isCatching;

	public void RelasingAxe()
	{
		anim.SetBool("IsTrowing", true);
	}

	public void RetrievingAxe()
	{
		anim.SetBool("IsRetrieving", true);
	}
	public void startCharging()
	{
		anim.SetBool("IsCharging", true);
	}
	public void StopCharging()
	{
		anim.SetBool("IsCharging", false);
	}
	public void ToggleCatching()
	{
		isCatching = !isCatching;
		anim.SetBool("isCatching", isCatching);
	}
	public void AttackSpin()
	{
		AnimReset();
		anim.SetBool("Is360", true);
		Invoke("AnimReset", 2);
	}
	public void TurnOffTrowingBool()
	{
		anim.SetBool("IsTrowing", false);
		anim.SetBool("IsCharging", false);
		anim.SetBool("IsRetrieving", false);
		anim.SetBool("isCatching", false);
	}
	public void Kick()
	{
		anim.SetBool("IsKicking", true);
		Invoke("AnimReset", 1);
	}

	public void NormalAttack()
	{
		anim.SetBool("IsAttacking", true);
		Invoke("AnimReset", 2.2f);
	}

	public void Taunt()
	{
		anim.SetBool("IsTaunting", true);
		Invoke("AnimReset", 2.8f);
	}

	public void AnimReset()
	{
		anim.SetBool("IsTaunting", false);
		anim.SetBool("IsKicking", false);
		anim.SetBool("Is360", false);
		anim.SetBool("IsAttacking", false);
	}
	public void SetVelocity(float velocitiyZ, float velocitiyX)
	{
		anim.SetFloat("Velocity z", velocitiyZ);
		anim.SetFloat("Velocity x", velocitiyX);
	}
	public void SetAnimLayerWeight(int layer,float weight)
	{
		anim.SetLayerWeight(layer, weight);
	}
}
