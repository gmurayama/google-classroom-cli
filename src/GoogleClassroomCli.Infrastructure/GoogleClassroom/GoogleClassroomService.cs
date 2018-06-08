using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Services;
using System.IO;

namespace GoogleClassroomCli.Infrastructure.GoogleClassroom
{
    public abstract class GoogleClassroomService
    {
        private GoogleCredential _credential;
        private string _credentialEmail;
        private string[] _scopes;
        private ClassroomService _classroomService;

        public string CredentialEmail => _credentialEmail;

        protected ClassroomService Service
        {
            get
            {
                if (_classroomService == null)
                {
                    InitializeClassroomService();
                    return _classroomService;
                }
                else
                    return _classroomService;
            }
        }

        public GoogleClassroomService(string privateKeyPath)
        {
            GenerateCredentialFromPrivateKey(privateKeyPath);
        }

        public GoogleClassroomService(string privateKeyPath, string emailToImpersonate) : this(privateKeyPath)
        {
            ImpersonateEmail(emailToImpersonate);
        }

        public GoogleClassroomService(string privateKeyPath, string emailToImpersonate, params string[] scopes) : this(privateKeyPath, emailToImpersonate)
        {
            SetScopes(scopes);
        }

        private void GenerateCredentialFromPrivateKey(string privateKeyPath)
        {
            using (var stream = new FileStream(@privateKeyPath, FileMode.Open, FileAccess.Read))
            {
                _credential = GoogleCredential.FromStream(stream);
            }
        }

        public void ImpersonateEmail(string email, params string[] scopes)
        {
            _credential = _credential.CreateWithUser(email);
            _credentialEmail = email;

            if (scopes?.Length != 0)
                _scopes = scopes;

            InitializeClassroomService();
        }

        public void SetScopes(params string[] scopes)
        {
            _scopes = scopes;
            InitializeClassroomService();
        }

        private void InitializeClassroomService()
        {
            _classroomService = new ClassroomService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential.CreateScoped(_scopes)
            });
        }
    }
}
