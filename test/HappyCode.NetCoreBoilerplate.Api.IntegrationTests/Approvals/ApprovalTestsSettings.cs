using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.TestFrameworks;

[assembly: UseApprovalSubdirectory("Approvals")]
[assembly: UseReporter(typeof(XUnit2Reporter), typeof(DiffReporter))]
