public interface IUnit
{
	void TakeDamage(int rawDamage);
	void ReceiveHeal(int rawHealAmount);
	void Stun(float stunDuration);

	bool IsDead();
	bool IsStunned();
	int GetCurrentHealth();
	int GetHealingPower();
	int GetAttackPower();

	bool IsBadGuy();
	bool IsGoodGuy();
}
