module App { 

$Classes(*)[ // Find all classes with a name ending with Model
  export  class $Name {
        constructor($Properties[public $name: $Type][, ]) {
        }
    }
] 

}