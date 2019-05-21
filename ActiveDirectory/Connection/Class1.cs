using System;
using System.DirectoryServices;

namespace Connection
{
    internal static class Globals
    {

        public static int SOCKET = 389;
        public static string SERVERNAME = "server1";
        public static string USERNAME = "Administrador";
        public static string PASSWORD = "Server1";

        public static int getSocket
        {
            get { return SOCKET; }
        }

        public static string getServerName
        {
            get { return SERVERNAME; }
        }

        public static string getUserName
        {
            get { return USERNAME; }
        }

        public static string getPassword
        {
            get { return PASSWORD; }
        }
    }

    public partial class ActiveDirectory
    {

        private DirectorySearcher dirSearch = null;

        public bool createUser(string name, string lastName, string user, string password, string emailAddress)
        {
            try
            {
                DirectoryEntry adUserFolder = new DirectoryEntry("LDAP://" + Globals.getServerName + ":" + Globals.getSocket.ToString() + "/CN=MRS,DC=CONTOSO,DC=COM",
                    Globals.getUserName, Globals.getPassword);

                SearchResult rs = null;
                rs = searchUserByUserName(getDirectorySearcher(), user);

                if (rs != null)
                {
                    throw new Exception("El usuario ya existe.");
                }

                DirectoryEntry newUser = adUserFolder.Children.Add("CN=" + name + " " + lastName, "user");

                using (newUser)
                {
                    newUser.Properties["mail"].Value = emailAddress;
                    newUser.Properties["uid"].Value = user;
                    newUser.Properties["x500uniqueIdentifier"].Value = password;
                    newUser.CommitChanges();
                }

            }
            catch (Exception e)
            {
                throw new Exception("Error en el proceso de creación.\n" + "Más detalles del error: \n" + e.Message.ToString());
            }

            return true;
        }

        public bool authentication(string user, string password)
        {

            SearchResult rs = null;
            if (user.IndexOf("@") > 0)
                rs = searchUserByEmail(getDirectorySearcher(), user);
            else
                rs = searchUserByUserName(getDirectorySearcher(), user);

            if (rs != null)
            {
                if (rs.GetDirectoryEntry().Properties["uid"].Value.ToString() != user ||
                    rs.GetDirectoryEntry().Properties["x500uniqueIdentifier"].Value.ToString() != password)
                    throw new Exception("Credenciales incorrectas.");
            }
            else
            {
                throw new Exception("Usuario no encontrado.");
            }

            return true;
        }

        private DirectorySearcher getDirectorySearcher()
        {
            if (dirSearch == null)
            {
                try
                {
                    dirSearch = new DirectorySearcher(
                        new DirectoryEntry("LDAP://" + Globals.getServerName + ":" + Globals.getSocket.ToString() + "/CN=MRS,DC=CONTOSO,DC=COM",
                        Globals.getUserName, Globals.getPassword));
                }
                catch (DirectoryServicesCOMException e)
                {
                    throw new Exception("Falló la conexion.\n" + "Más detalles del error: \n" + e.Message.ToString());
                }
                return dirSearch;
            }
            else
            {
                return dirSearch;
            }
        }

        private SearchResult searchUserByUserName(DirectorySearcher ds, string username)
        {
            ds.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(uid=" + username + "))";

            ds.SearchScope = SearchScope.Subtree;
            ds.ServerTimeLimit = TimeSpan.FromSeconds(90);

            SearchResult userObject = ds.FindOne();

            if (userObject != null)
                return userObject;
            else
                return null;
        }

        private SearchResult searchUserByEmail(DirectorySearcher ds, string email)
        {
            ds.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(mail=" + email + "))";

            ds.SearchScope = SearchScope.Subtree;
            ds.ServerTimeLimit = TimeSpan.FromSeconds(90);

            SearchResult userObject = ds.FindOne();

            if (userObject != null)
                return userObject;
            else
                return null;
        }

    }

}
