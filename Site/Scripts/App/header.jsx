class Header extends React.Component {

    render() {
        return <div className="navbar navbar-inverse navbar-fixed-top">
            <div className="navbar-header">
                <button type="button" className="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span className="icon-bar"></span>
                    <span className="icon-bar"></span>
                    <span className="icon-bar"></span>
                </button>
                <a href="/" className="navbar-brand">All Acu</a>
            </div>

            <div className="navbar-collapse collapse" ng-controller="nav">
                <ul className="nav navbar-nav">
                    <li ng-repeat="item in navItems()"><a href="#">Buns</a></li>
                </ul>
                <ul className="nav navbar-nav navbar-right">
                    <li className="providers" ng-controller="providerChooser">
                        <span style={{color: "white" }}>Provider: </span>
                        <span>
                            <span style={{color: "white" }} ng-show="shouldDisplay() && !canChoose()">Needles</span>
                            <select className="form-control"
                                    ng-show="canChoose()"
                                    ng-model="currentProvider"
                                    ng-model-options="{ getterSetter: true }"
                                    ng-options="p.businessName for p in providers()"></select>
                        </span>
                    </li>
                    <li ng-show="loggedIn()"><a href="/AllAcu/#/logout">Logout</a></li>
                </ul>
            </div>
        </div>
    }
}