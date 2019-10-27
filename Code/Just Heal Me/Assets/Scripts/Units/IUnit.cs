public interface IUnit
{
	void TakeDamage(int rawDamage);
	void ReceiveHeal(int rawHealAmount);
	//void GetStunned(float stunDuration);

	bool IsDead();
	int GetCurrentHealth();
	int GetHealingPower();
	int GetAttackPower();

	bool IsBadGuy();
	bool IsGoodGuy();
}
