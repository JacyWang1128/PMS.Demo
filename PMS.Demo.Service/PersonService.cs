using PMS.Demo.Entity;
using PMS.Demo.Runtime;
using PMS.Demo.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Demo.Service
{
    public class PersonService : IEntity<Person>
    {

        private string tablename = "t_person";
        //Person CRUD
        public int Delete(Person t)
        {
            return Global.dbhelper.ExecuteNonQuery($"DELETE FROM {tablename} WHERE id = '{t.id}'");
        }

        public int Insert(Person t)
        {
            return Global.dbhelper.ExecuteNonQuery($"INSERT INTO 'main'.'{tablename}' ('id', 'name', 'gender', 'age', 'photopath', 'birthday') VALUES ' + $'('{t.id}', '{t.name}', '{t.gender}', {t.age}, '{t.photopath}', '{t.birthday}'");
        }

        public Person Query(string id)
        {
            return Global.dbhelper.ExecuteScalar<Person>($"SELECT * FROM {tablename} WHERE id = '{id}'");
        }

        public List<Person> QueryList()
        {
            return Global.dbhelper.ExecuteNonQuery<Person>(new string[] { $"SELECT * FROM {tablename}" }).ToList();
        }

        public int Update(Person t)
        {
            return Global.dbhelper.ExecuteNonQuery($"UPDATE 'main'.'{tablename}' SET 'name' = '{t.name}', 'gender' = '{t.gender}', 'age' = {t.age}, 'photopath' = '{t.photopath}', 'birthday' = '{t.birthday}' WHERE 'id' = '{t.id}'");
        }

        
    }
}
