using SheetSeller.Repositories.Implement;

namespace TestProject1
{
    public class Tests
    {
        private Payer _payer = new Payer();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Arrange
            var data = "eyJwYXltZW50X2lkIjoyMzM1NTY1Nzg0LCJhY3Rpb24iOiJwYXkiLCJzdGF0dXMiOiJzdWNjZXNzIiwidmVyc2lvbiI6MywidHlwZSI6ImJ1eSIsInBheXR5cGUiOiJjYXJkIiwicHVibGljX2tleSI6InNhbmRib3hfaTYyMTIyNjE0NTI1IiwiYWNxX2lkIjo0MTQ5NjMsIm9yZGVyX2lkIjoiUXExMlsxNV00NDgiLCJsaXFwYXlfb3JkZXJfaWQiOiJFTVIxMlQwVjE2ODg1NTIyODcxODMxNjciLCJkZXNjcmlwdGlvbiI6IlRlc3QgUGF5bWVudCIsInNlbmRlcl9waG9uZSI6IjM4MDk3OTQ0Nzk1MCIsInNlbmRlcl9jYXJkX21hc2syIjoiNTE2ODc1Kjc1Iiwic2VuZGVyX2NhcmRfYmFuayI6InBiIiwic2VuZGVyX2NhcmRfdHlwZSI6Im1jIiwic2VuZGVyX2NhcmRfY291bnRyeSI6ODA0LCJpcCI6IjE4NS4yMzIuMjAuOTAiLCJhbW91bnQiOjEuMCwiY3VycmVuY3kiOiJVU0QiLCJzZW5kZXJfY29tbWlzc2lvbiI6MC4wLCJyZWNlaXZlcl9jb21taXNzaW9uIjowLjAyLCJhZ2VudF9jb21taXNzaW9uIjowLjAsImFtb3VudF9kZWJpdCI6MzcuNDUsImFtb3VudF9jcmVkaXQiOjM3LjQ1LCJjb21taXNzaW9uX2RlYml0IjowLjAsImNvbW1pc3Npb25fY3JlZGl0IjowLjU2LCJjdXJyZW5jeV9kZWJpdCI6IlVBSCIsImN1cnJlbmN5X2NyZWRpdCI6IlVBSCIsInNlbmRlcl9ib251cyI6MC4wLCJhbW91bnRfYm9udXMiOjAuMCwibXBpX2VjaSI6IjciLCJpc18zZHMiOmZhbHNlLCJsYW5ndWFnZSI6InVrIiwiY3JlYXRlX2RhdGUiOjE2ODg1NTIyODcxODUsImVuZF9kYXRlIjoxNjg4NTUyMjg3MjcwLCJ0cmFuc2FjdGlvbl9pZCI6MjMzNTU2NTc4NH0=";
            var signature = "fowcA+DLQET4Bzq7Cbv0YC9AhhE=";

            // Act
            var res = _payer.AcceptPayment(data, signature);

            // Assert
            Assert.AreEqual(1, res.StatusCode);

        }
        [Test]
        public void Test2()
        {
            // Arrange
            var data = "eyJwYXltZW50X2lkIjoyMzM1NTY1Nzg0LCJhY3Rpb24iOiJwYXkiLCJzdGF0dXMiOiJzdWNjZXNzIiwidmVyc2lvbiI6MywidHlwZSI6ImJ1eSIsInBheXR5cGUiOiJjYXJkIiwicHVibGljX2tleSI6InNhbmRib3hfaTYyMTIyNjE0NTI1IiwiYWNxX2lkIjo0MTQ5NjMsIm9yZGVyX2lkIjoiUXExMlsxNV00NDgiLCJsaXFwYXlfb3JkZXJfaWQiOiJFTVIxMlQwVjE2ODg1NTIyODcxODMxNjciLCJkZXNjcmlwdGlvbiI6IlRlc3QgUGF5bWVudCIsInNlbmRlcl9waG9uZSI6IjM4MDk3OTQ0Nzk1MCIsInNlbmRlcl9jYXJkX21hc2syIjoiNTE2ODc1Kjc1Iiwic2VuZGVyX2NhcmRfYmFuayI6InBiIiwic2VuZGVyX2NhcmRfdHlwZSI6Im1jIiwic2VuZGVyX2NhcmRfY291bnRyeSI6ODA0LCJpcCI6IjE4NS4yMzIuMjAuOTAiLCJhbW91bnQiOjEuMCwiY3VycmVuY3kiOiJVU0QiLCJzZW5kZXJfY29tbWlzc2lvbiI6MC4wLCJyZWNlaXZlcl9jb21taXNzaW9uIjowLjAyLCJhZ2VudF9jb21taXNzaW9uIjowLjAsImFtb3VudF9kZWJpdCI6MzcuNDUsImFtb3VudF9jcmVkaXQiOjM3LjQ1LCJjb21taXNzaW9uX2RlYml0IjowLjAsImNvbW1pc3Npb25fY3JlZGl0IjowLjU2LCJjdXJyZW5jeV9kZWJpdCI6IlVBSCIsImN1cnJlbmN5X2NyZWRpdCI6IlVBSCIsInNlbmRlcl9ib251cyI6MC4wLCJhbW91bnRfYm9udXMiOjAuMCwibXBpX2VjaSI6IjciLCJpc18zZHMiOmZhbHNlLCJsYW5ndWFnZSI6InVrIiwiY3JlYXRlX2RhdGUiOjE2ODg1NTIyODcxODUsImVuZF9kYXRlIjoxNjg4NTUyMjg3MjcwLCJ0cmFuc2FjdGlvbl9pZCI6MjMzNTU2NTc4NH0=";
            var signature = "fowcA+DLQET4Bzq7Cbv0YC9AhhE";

            // Act
            var res = _payer.AcceptPayment(data, signature);

            // Assert
            Assert.AreNotEqual(1, res.StatusCode);

        }
        [Test]
        public void Test3()
        {
            // Arrange
            var data = "yJwYXltZW50X2lkIjoyMzM1NTY1Nzg0LCJhY3Rpb24iOiJwYXkiLCJzdGF0dXMiOiJzdWNjZXNzIiwidmVyc2lvbiI6MywidHlwZSI6ImJ1eSIsInBheXR5cGUiOiJjYXJkIiwicHVibGljX2tleSI6InNhbmRib3hfaTYyMTIyNjE0NTI1IiwiYWNxX2lkIjo0MTQ5NjMsIm9yZGVyX2lkIjoiUXExMlsxNV00NDgiLCJsaXFwYXlfb3JkZXJfaWQiOiJFTVIxMlQwVjE2ODg1NTIyODcxODMxNjciLCJkZXNjcmlwdGlvbiI6IlRlc3QgUGF5bWVudCIsInNlbmRlcl9waG9uZSI6IjM4MDk3OTQ0Nzk1MCIsInNlbmRlcl9jYXJkX21hc2syIjoiNTE2ODc1Kjc1Iiwic2VuZGVyX2NhcmRfYmFuayI6InBiIiwic2VuZGVyX2NhcmRfdHlwZSI6Im1jIiwic2VuZGVyX2NhcmRfY291bnRyeSI6ODA0LCJpcCI6IjE4NS4yMzIuMjAuOTAiLCJhbW91bnQiOjEuMCwiY3VycmVuY3kiOiJVU0QiLCJzZW5kZXJfY29tbWlzc2lvbiI6MC4wLCJyZWNlaXZlcl9jb21taXNzaW9uIjowLjAyLCJhZ2VudF9jb21taXNzaW9uIjowLjAsImFtb3VudF9kZWJpdCI6MzcuNDUsImFtb3VudF9jcmVkaXQiOjM3LjQ1LCJjb21taXNzaW9uX2RlYml0IjowLjAsImNvbW1pc3Npb25fY3JlZGl0IjowLjU2LCJjdXJyZW5jeV9kZWJpdCI6IlVBSCIsImN1cnJlbmN5X2NyZWRpdCI6IlVBSCIsInNlbmRlcl9ib251cyI6MC4wLCJhbW91bnRfYm9udXMiOjAuMCwibXBpX2VjaSI6IjciLCJpc18zZHMiOmZhbHNlLCJsYW5ndWFnZSI6InVrIiwiY3JlYXRlX2RhdGUiOjE2ODg1NTIyODcxODUsImVuZF9kYXRlIjoxNjg4NTUyMjg3MjcwLCJ0cmFuc2FjdGlvbl9pZCI6MjMzNTU2NTc4NH0=";
            var signature = "fowcA+DLQET4Bzq7Cbv0YC9AhhE=";

            // Act
            var res = _payer.AcceptPayment(data, signature);

            // Assert
            Assert.AreNotEqual(1, res.StatusCode);

        }
        [Test]
        public void Test4()
        {
            // Arrange
            var amount = 1;
            string order = "234[Nazarchik]";

            // Act
            var payment = _payer.CreatePayment(amount, order);
            var res = _payer.AcceptPayment(payment["data"], payment["signature"]);

            // Assert
            Assert.AreEqual(1, res.StatusCode);

        }
    }
}