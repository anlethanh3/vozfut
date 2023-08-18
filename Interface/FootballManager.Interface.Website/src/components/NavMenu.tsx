import { useState } from "react";
import { Container, Navbar, Nav, Button, Dropdown, Image, Row, Col, DropdownButton } from "react-bootstrap";
import Login from "./Login";
import { selectProfile, signOut, } from '../slices/profileSlice';
import { useAppSelector, useAppDispatch } from '../app/hooks';

export default function NavMenu() {
    const [isLogin, setIsLogin] = useState(false)
    const profile = useAppSelector(selectProfile)
    const dispatch = useAppDispatch()
    const imageId = '1oFAxwi-7N4QM2VJ-t9r2fAItp-DiSm8n'
    function imageUri(imageId: string) {
        return `https://drive.google.com/uc?export=view&id=${imageId}`
    }

    function showProfile() {
        return (
            <Row className="d-inline-block">
                <Col>
                    <Image className="px-0" style={{ width: '40px' }} src={imageUri(imageId)} roundedCircle />
                </Col>
                {/* <Col>
                    <p className="mx-0 mp-0">{profile?.username ?? ''}</p>
                </Col> */}
            </Row>
        )
    }

    function Profile() {
        if (profile === undefined) {
            return (
                <Button onClick={() => setIsLogin(true)}>Login</Button>
            )
        }
        return (
            <DropdownButton flip id="dropdown-basic-button" title={showProfile()}>
                <Dropdown.Header>
                    <Image style={{ width: '100px' }} src={imageUri(imageId)} roundedCircle />
                    <Dropdown.ItemText>Email: {profile.email}</Dropdown.ItemText>
                    <Dropdown.ItemText>Role: {profile.role}</Dropdown.ItemText>
                    <Dropdown.ItemText>Elo: +5</Dropdown.ItemText>
                </Dropdown.Header>
                <Dropdown.Divider />
                <Dropdown.Item>Chỉ số chi tiết</Dropdown.Item>
                <Dropdown.ItemText>Giữ bóng: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Sút xa: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Dứt điểm: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Chuyền bóng: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Tốc độ chạy: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Tăng tốc: 99</Dropdown.ItemText>
                <Dropdown.Divider />
                <Dropdown.ItemText>Chỉ số ẩn:</Dropdown.ItemText>
                <Dropdown.ItemText>5* Gáy</Dropdown.ItemText>
                <Dropdown.ItemText>5* Câu giờ</Dropdown.ItemText>
                <Dropdown.ItemText>Leadership</Dropdown.ItemText>
                <Dropdown.Divider />
                <Dropdown.Item onClick={() => dispatch(signOut())}>
                    Sign Out
                </Dropdown.Item>
            </DropdownButton >
        )
    }

    return (
        <>
            {
                isLogin &&
                <Login show={isLogin} onClose={() => setIsLogin(false)} onSubmit={(data) => {
                    setIsLogin(false)
                }} />
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
                    <Navbar.Collapse >
                        <Nav className="me-auto">
                            <Nav.Link href="/">Home</Nav.Link>
                            <Nav.Link href="match">Match</Nav.Link>
                            <Nav.Link href="member">Member</Nav.Link>
                            <Nav.Link href="donate">Donate</Nav.Link>
                            <Nav.Link href="history">History</Nav.Link>
                            <Nav.Link href="counter">Counter</Nav.Link>
                        </Nav>
                        <Nav>
                            <Profile />
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </>
    )
}