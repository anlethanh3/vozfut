import { useState } from "react";
import { Container, Navbar, Nav, Button } from "react-bootstrap";
import Login from "./Login";

export default function NavMenu() {
    const [isLogin, setIsLogin] = useState(false)

    return (
        <>
            {
                isLogin &&
                <Login show={isLogin} onClose={() => setIsLogin(false)} onSubmit={(data) => {}} />
            }
            <Navbar expand="md" bg="primary" data-bs-theme="dark">
                <Container>
                    <Navbar.Brand href="/">
                        <img
                            alt=""
                            src="/icon.jpg"
                            width="30"
                            height="30"
                            className="d-inline-block align-top"
                        />{' '}Voz Football Club</Navbar.Brand>
                    <Navbar.Toggle />
                    <Navbar.Collapse className="justify-content-end">
                        <Nav className="me-auto">
                            <Nav.Link href="/">Home</Nav.Link>
                            <Nav.Link href="match">Match</Nav.Link>
                            <Nav.Link href="member">Member</Nav.Link>
                            <Nav.Link href="donate">Donate</Nav.Link>
                            <Nav.Link href="history">History</Nav.Link>
                            <Nav.Link href="counter">Counter</Nav.Link>
                        </Nav>
                        <Nav>
                            <Button onClick={() => setIsLogin(true)}>Login</Button>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </>
    )
}