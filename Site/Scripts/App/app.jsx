class App extends React.Component {
    render() {
        return <div>
            <Header></Header>
            <h4>I'm a wing app</h4>
            <Footer copyrightYear="2001"></Footer>
        </div>
    }
}

ReactDOM.render(
    <App>Hello, app!</App>,
    document.getElementById('content')
);
